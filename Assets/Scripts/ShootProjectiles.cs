using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectiles : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public static GameObject selectedProjectilePrefab;
    public float projectileSpeed = 10f;
    public Image crosshair;
    public Color initialCrosshairColor;
    public Color target2DetectionColor;

    // Start is called before the first frame update
    void Start()
    {
        initialCrosshairColor = crosshair.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PausedMenuBehavior.isGamePaused)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject projectile =
                    Instantiate(selectedProjectilePrefab, transform.position + transform.forward, transform.rotation)
                    as GameObject;

                if (!projectile.GetComponent<Rigidbody>())
                {
                    projectile.AddComponent<Rigidbody>();
                }

                float speed = projectile.GetComponent<ProjectileSpeed>().projectileSpeed;
                projectile.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.VelocityChange);

                projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            }

            crosshair.enabled = true;
            CrosshairEffect();
        }
        else
        {
            crosshair.enabled = false;
        }
        
    }

    void CrosshairEffect(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity)){
            if (hit.collider.CompareTag("Enemy"))
            {
                selectedProjectilePrefab = projectilePrefab;
                crosshair.color = new Color(initialCrosshairColor.r,
                    initialCrosshairColor.g,
                    initialCrosshairColor.b,
                    1f);

                crosshair.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else if (hit.collider.CompareTag("Reducto"))
            {
                selectedProjectilePrefab = projectilePrefab2;
                crosshair.color = new Color(target2DetectionColor.r,
                    target2DetectionColor.g,
                    target2DetectionColor.b,
                    1f);

                crosshair.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else
            {
                selectedProjectilePrefab = projectilePrefab;
                crosshair.color = Color.Lerp(crosshair.color, initialCrosshairColor, 2 * Time.deltaTime);
                crosshair.transform.localScale = Vector3.Lerp(crosshair.transform.localScale, new Vector3(1, 1, 1), 2 * Time.deltaTime);
            }
        }
        else
        {
            selectedProjectilePrefab = projectilePrefab;
            crosshair.color = Color.Lerp(crosshair.color, initialCrosshairColor, 2 * Time.deltaTime);
            crosshair.transform.localScale = Vector3.Lerp(crosshair.transform.localScale, new Vector3(1, 1, 1), 2 * Time.deltaTime);
        }
    }
}
