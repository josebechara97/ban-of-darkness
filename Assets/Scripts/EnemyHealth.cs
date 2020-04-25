using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deadSFX;
    public Slider healthbar;


    private void Awake()
    {
        healthbar = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealthBar()
    {
        healthbar.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            UpdateHealthBar();
            print("health: " + currentHealth);
        }
        if (currentHealth <= 0)
        {
           //dead!
        }
    }

    public void PlayerDies()
    {
        print("Player is dead");
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        transform.Rotate(-90f, 0f, 0f, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(10);
            EnemyAI enemyAI = gameObject.GetComponent<EnemyAI>();
            enemyAI.nextDestination = enemyAI.player.transform.position;
        }
        if (collision.gameObject.CompareTag("Knock"))
        {
            TakeDamage(1);
            EnemyAI enemyAI = gameObject.GetComponent<EnemyAI>();
            enemyAI.currentState = EnemyAI.FSMStates.KnockedDown;
        }
    }
}
