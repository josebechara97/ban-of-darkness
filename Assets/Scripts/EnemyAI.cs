using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Patrol,
        Chase,
        Attack,
        Dead,
        KnockedDown

    }

    public FSMStates currentState;
    public GameObject[] wanderPoints;
    public Vector3 nextDestination;
    private int currentDestIndex = 0;
    public float speed = 3.5f;
    private Animator anim;
    public float rotationSpeed = 5f;
    public GameObject player;
    public float distanceToPlayer;
    public float attackDistance = 20;
    public float chaseDistance = 30;
    public GameObject[] spellProjectiles;
    public GameObject wandTip;
    public float projectileSpeed;
    public AudioClip shootSFX;
    public float shootRate = 2;
    public float elapsedTime = 0;
    EnemyHealth enemyHealth;
    public int health;
    public GameObject deadVFX;
    Transform deadTransform;
    bool isDead;
    private NavMeshAgent agent;
    public float chaseSpeed = 5f;
    public GameObject enemyEyes;
    public float fieldOfView = 45f;
    public float knockedDownTime = 3f;
    public bool knockedDownOnce = false;


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        anim = GetComponent<Animator>();

        if (wanderPoints.Length < 1)
            wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;
        agent = GetComponent<NavMeshAgent>();
        Initialize();
    }

    private void Initialize()
    {
        FindNextPoint();
        currentState = FSMStates.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                {
                    UpdatePatrolState();
                    break;
                }
            case FSMStates.Chase:
                {
                    UpdateChaseState();
                    break;
                }
            case FSMStates.Attack:
                {
                    UpdateAttackState();
                    break;
                }
            case FSMStates.Dead:
                {
                    UpdateDeadState();
                    break;
                }
            case FSMStates.KnockedDown:
                {
                    UpdateKnockedDown();
                    break;
                }
        }
        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }

    }

    private void UpdateKnockedDown()
    {
        anim.SetInteger("animState", 5);
        agent.velocity = Vector3.zero;
        if (!knockedDownOnce)
        {
            Invoke("ExitKnockedDown", knockedDownTime);
            knockedDownOnce = true;
        }
    }

    private void ExitKnockedDown()
    {
        knockedDownOnce = false;
        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
        else if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance &&
            distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }
    }

    private void UpdateAttackState()
    {
        anim.SetInteger("animState", 3);
        agent.stoppingDistance = attackDistance;
        agent.velocity = Vector3.zero;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance &&
            distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        EnemySpellCast();
    }

    private void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);
        agent.stoppingDistance = attackDistance;
        agent.speed = chaseSpeed;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        nextDestination = player.transform.position;
        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);

    }

    private void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);
        agent.stoppingDistance = 0;
        agent.speed = speed;

        if (Vector3.Distance(transform.position, nextDestination) < 3)
        {
            FindNextPoint();
        }
        else if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    private void UpdateDeadState()
    {
        agent.velocity = Vector3.zero;
        isDead = true;
        anim.SetInteger("animState", 4);
        deadTransform = transform;
        Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestIndex].transform.position;
        currentDestIndex = (currentDestIndex + 1) % wanderPoints.Length;
        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime); ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.transform.position + (enemyEyes.transform.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.transform.position, frontRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.transform.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.transform.position, rightRayPoint, Color.yellow);
    }

    void EnemySpellCast()
    {
        if (elapsedTime >= shootRate && !isDead)
        {
            var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            Invoke("SpellCasting", animDuration);
            elapsedTime = 0.00f;
        }
    }

    void SpellCasting()
    {
        if (!isDead && !knockedDownOnce)
        {
            GameObject spellProjectile = spellProjectiles[UnityEngine.Random.Range(0, spellProjectiles.Length - 1)];
            Instantiate(spellProjectile, this.wandTip.transform.position, wandTip.transform.rotation);
        }
    }

    private void OnDestroy()
    {
        try
        {
            if (deadVFX == null)
                print("dead sfx null at: " + gameObject.name);
            if (deadTransform == null)
                print("dead transform null at: " + gameObject.name);
            Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
        }catch(Exception e)
        {
            print("exception at " + gameObject.name);
        }
        
    }

    private bool IsPlayerInClearFOV()
    {
        Vector3 directionToPlayer = player.transform.position - enemyEyes.transform.position;
        RaycastHit hit;
        if (Vector3.Angle(directionToPlayer, enemyEyes.transform.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.transform.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }

            }
        }

        return false;
    }

}
