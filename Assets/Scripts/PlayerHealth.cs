using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthbar;


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
        if(currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    public void PlayerDies()
    {
        print("Player is dead");
        transform.Rotate(-90f, 0f, 0f, Space.Self);
        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().LevelLost();
    }
}
