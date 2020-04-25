using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTagsOnCollision : MonoBehaviour
{
    public string[] tags;

    private void OnTriggerEnter(Collider other)
    {
        print("collision :" + other.gameObject.tag);

        if (TagMatch(other.gameObject.tag))
        {
            other.gameObject.transform.parent = transform;
            print("match with :" + other.gameObject.tag);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        print("collision :" + other.gameObject.tag);

        if (TagMatch(other.gameObject.tag))
        {
            other.gameObject.transform.parent = transform;
            print("match with :" + other.gameObject.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("collision :" + other.gameObject.tag);

        if (TagMatch(other.gameObject.tag))
        {
            other.gameObject.transform.parent = null;
            print("match with :" + other.gameObject.tag);
        }
    }


    private bool TagMatch(string tag)
    {
        foreach(string t in tags)
        {
            if (t.Equals(tag))
            {
                return true;
            }
        }

        return false;
    }
}
