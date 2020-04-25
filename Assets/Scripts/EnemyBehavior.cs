using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Transform target;
    public Vector3 accelerationTarget;
    public bool accelerationTargetReady = false;
    public float speed = 1f;
    public float accelerationSpeed = 10f;
    public float distanceToTarget = float.MaxValue;
    public float targetDetectionRadious = 20f;
    public float targetAccelerationRadious = 10f;
    public AudioClip SFXAcceleration;
    PlayerHealth playerHealth;
    public int damageAmount = 25;
    public GameObject explosion;
    private bool changeRequested;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (target != null && distanceToTarget < targetDetectionRadious)
        {

            if (distanceToTarget < targetAccelerationRadious)
            {
                if (!accelerationTargetReady)
                {
                    AudioSource.PlayClipAtPoint(SFXAcceleration, transform.position);
                    accelerationTarget = target.position;
                    accelerationTargetReady = true;
                    changeRequested = false;
                }
                if (Vector3.Distance(transform.position, accelerationTarget) < 0.01f && !changeRequested)
                {
                    Invoke("ChangeAccelerationTarget", 2f);
                    changeRequested = true;
                }
                this.transform.LookAt(accelerationTarget);
                this.transform.position = Vector3.MoveTowards(transform.position, accelerationTarget, accelerationSpeed * Time.deltaTime);
            }
            else
            {
                accelerationTargetReady = false;
                this.transform.LookAt(target);
                this.transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }

        }
    }

    public void ChangeAccelerationTarget()
    {
        accelerationTargetReady = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            playerHealth.TakeDamage(damageAmount);
            GetComponent<KillableByTag>().TriggerDestroy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            playerHealth.TakeDamage(damageAmount);
            GetComponent<KillableByTag>().TriggerDestroy();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetAccelerationRadious);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRadious);
    }
}
