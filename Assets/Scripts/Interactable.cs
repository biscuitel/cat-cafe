using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    private bool playerInTrigger;
    private LineRenderer lr;
    private int raycastLayerMask;
    private Camera cam;
    private GameObject obj;
    private TaskManager taskManager;

    [SerializeField] private int taskID;


    // max distance that player can interact from
    public float interactionRange = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        raycastLayerMask = 1 << this.gameObject.layer;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInTrigger = false;
        obj = this.transform.GetChild(0).gameObject;
        taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    private void CheckForInteraction()
    {
        // if player is in trigger for this object, and presses interact
        if (playerInTrigger && Input.GetButtonDown("Interact"))
        {
            Debug.Log("Player is in trigger and pressed interact button");

            // get camera center
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // draw line of raycast for debugging purposes
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactionRange, Color.yellow, 5f);

            // cast ray from camera center, if hits this object then interact with it
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("raycast hit something");
                if (GameObject.ReferenceEquals(obj, hit.transform.gameObject))
                {
                    Debug.Log("Player raycast hit this object");
                    Interact();
                }
            }

        }
    }

    private void Interact()
    {
        Debug.Log("Player interacted with me! Do a thing or smth idk");
        taskManager.TaskComplete(taskID);
        //do thing here

    }


    // check whether player is in the trigger that this script is attached to
    void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player") && !playerInTrigger)
            {
                Debug.Log("Player entered trigger for interactable object");
                playerInTrigger = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            // set interactable 
            if (other.CompareTag("Player") && playerInTrigger)
            {
                Debug.Log("Player exited trigger for interactable object");
                playerInTrigger = false;
            }
        }
    }
}
