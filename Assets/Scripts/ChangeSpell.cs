using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpell : MonoBehaviour
{
    public GameObject[] spellProjectiles;
    int selectedSpell = 0;
    GameObject currentProjectile;
    public GameObject spellPanel;
    Button[] buttons;


    // Start is called before the first frame update
    void Start()
    {
        buttons = spellPanel.GetComponentsInChildren<Button>();
        UpdateSpellUI();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSpell = selectedSpell;
        if(Input.GetAxis("Mouse ScrollWheel") < 0f||Input.GetKeyDown(KeyCode.E))
        {
            //scrolling down
            if (selectedSpell >= spellProjectiles.Length-1)
            {
                selectedSpell = 0;
            }
            else
            {
                selectedSpell++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Q))
        {
            //scrolling up
            if (selectedSpell <= 0)
            {
                selectedSpell = spellProjectiles.Length - 1;
            }
            else
            {
                selectedSpell--;
            }
        }
        currentProjectile = spellProjectiles[selectedSpell];
        ShootProjectiles.selectedProjectilePrefab = currentProjectile;

        if (previousSpell != selectedSpell)
        {
            UpdateSpellUI();
        }
    }

    void UpdateSpellUI()
    {
        int i = 0;
        foreach(Button spellIcon in buttons)
        {
            if (i == selectedSpell)
                spellIcon.transform.localScale *= 1.25f;

            else
                spellIcon.transform.localScale = new Vector3(1, 1, 1);

            i++;       
        }
    }
}
