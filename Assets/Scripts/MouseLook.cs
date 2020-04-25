using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensivity = 100f;
    float xRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        // if null get transform from parent
        if(playerBody == null)
        {
            playerBody = transform.parent.transform;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PausedMenuBehavior.isGamePaused)
        {
            float moveX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

            playerBody.Rotate(Vector3.up * moveX);

            xRotation -= moveY;
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}
