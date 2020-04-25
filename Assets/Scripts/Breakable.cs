using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public string breakableByTag = "Projectile";
    public GameObject piecesPrefab;
    public float explosionForce = 50f;
    public float explosionRange = 2f;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(breakableByTag))
        {
            Transform currentTransform = gameObject.transform;
            
            Destroy(gameObject);

            GameObject pieces = Instantiate(piecesPrefab, currentTransform.position, currentTransform.rotation);

            Rigidbody[] rbPieces = pieces.GetComponentsInChildren<Rigidbody>();

            foreach(Rigidbody rb in rbPieces)
            {
                rb.AddExplosionForce(explosionForce, currentTransform.position, explosionRange);
            }
        }
    }

}
