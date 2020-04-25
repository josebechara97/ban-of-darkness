using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{

    public Vector3 initial;
    public Vector3 result;
    public float inSeconds;
    public float secondsPassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        initial = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (secondsPassed < inSeconds)
        {
            secondsPassed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initial, result, secondsPassed / inSeconds);
        }
    }
}
