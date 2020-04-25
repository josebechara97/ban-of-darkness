using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCrate : MonoBehaviour
{
    public float speed = 3;
    public float distance = 5;
    private Vector3 startPos;
    public enum Axis
    {
        X,
        Y,
        Z
    }
    public Axis direction;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = transform.position;
        switch (direction){
            case Axis.X:
                newPos.x = startPos.x + (Mathf.Sin(Time.time * speed)) * distance;
                break;
            case Axis.Y:
                newPos.y = startPos.y + (Mathf.Sin(Time.time * speed)) * distance;
                break;
            case Axis.Z:
                newPos.z = startPos.z + (Mathf.Sin(Time.time * speed)) * distance;
                break;
        }
        
        transform.position = newPos;
    }
}
