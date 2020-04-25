using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectileNoAim : MonoBehaviour
{

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public AudioClip shootSFX;
    public string button = "Fire1";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(button))
        {

            if (!projectilePrefab.GetComponent<Rigidbody>())
            {
                projectilePrefab.AddComponent<Rigidbody>();
            }

            projectilePrefab.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectilePrefab.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            AudioSource.PlayClipAtPoint(shootSFX, transform.position);
        }

    }
}
