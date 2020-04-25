using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyType;

    public float spawnTime = 3f;

    public float xMin = 0;

    public float xMax = 0;

    public float yMin = 0;

    public float yMax = 0;

    public float zMin = 0;

    public float zMax = 0;

    public float distanceToActivate = 15;

    public float distanceToPlayer = 0;

    public GameObject player;

    public float rotationSpeedActive = 20f;

    public float rotationSpeedInActive = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        InvokeRepeating("SpawnEnemies" //name of function
            , spawnTime //delay before first
            , spawnTime //delay before repeat
            );
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    void SpawnEnemies()
    {
        if (distanceToPlayer < distanceToActivate)
        {
            GetComponent<Rotate>().speed = rotationSpeedActive;
            Vector3 enemyPosition;
            enemyPosition.x = Random.Range(xMin, xMax);
            enemyPosition.y = Random.Range(yMin, yMax);
            enemyPosition.z = Random.Range(zMin, zMax);

            GameObject newEnemy =
                Instantiate(enemyType, transform.position + enemyPosition, transform.rotation)
                as GameObject;

            //newEnemy.transform.parent = gameObject.transform;
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().startingEnemyCount++;
        }
        else
        {
            GetComponent<Rotate>().speed = rotationSpeedInActive;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToActivate);
    }
}
