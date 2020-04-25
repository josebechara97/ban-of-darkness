using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastProtego : MonoBehaviour
{
    public GameObject prefab;
    public AudioClip castSXF;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var protego = Instantiate(prefab);
            protego.transform.position = transform.position;
            protego.transform.rotation = transform.rotation;
            AudioSource.PlayClipAtPoint(castSXF, transform.position);
        }
        
    }
}
