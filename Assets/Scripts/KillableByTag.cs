using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillableByTag : MonoBehaviour
{
    public string dangerTag = "Projectile";
    public GameObject killedRemains;
    private Transform onDestroyPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(dangerTag))
        {
            TriggerDestroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(dangerTag))
        {
            TriggerDestroy();
        }
    }

    public void TriggerDestroy()
    {
        onDestroyPosition = transform;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        try
        {
            if (killedRemains != null)
                Instantiate(killedRemains, onDestroyPosition.position, onDestroyPosition.rotation);
        }catch(Exception e)
        {
            print("exception on " + gameObject.name);
        }
        
    }
}
