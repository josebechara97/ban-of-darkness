using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellBehavior : MonoBehaviour
{
    PlayerHealth playerHealth;
    public int spellDamage = 10;

    void Start()
    {
        GameObject target = GameObject.FindWithTag("MainCamera");
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        
        transform.LookAt(target.transform);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(spellDamage);
        }
    }
}
