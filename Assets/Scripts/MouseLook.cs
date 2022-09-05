using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSens = 100f;

    private Transform playerBody;
    private float xRot = 0f;
    private DialogueManager dm;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = this.transform.parent.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        GameObject dmObj = GameObject.FindGameObjectWithTag("DialogueManager");
        if (dmObj != null)
        {
            dm = dmObj.GetComponent<DialogueManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dm == null || !dm.IsDialogueActive())
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSens;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens;

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
