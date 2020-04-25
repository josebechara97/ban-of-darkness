using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().enemiesKilled++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
