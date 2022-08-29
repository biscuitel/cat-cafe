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
    private DialogueTrigger dialogueTrigger;

    [SerializeField] private int taskID;
    [SerializeField] private bool deactivateAfterInteraction = false;
    [SerializeField] private bool deleteAfterInteraction = false;
    [SerializeField] private bool toggleVisAfterInteraction = false;

    [SerializeField] private MeshRenderer[] meshes;


    // max distance that player can interact from
    public float interactionRange = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        raycastLayerMask = 1 << this.gameObject.layer;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInTrigger = false;
        obj = this.transform.GetChild(0).gameObject;
        taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();
        dialogueTrigger = this.GetComponent<DialogueTrigger>();
        meshes = this.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    private void CheckForInteraction()
    {
        if (playerInTrigger)
        {
            // get camera center
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // draw line of raycast for debugging purposes
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactionRange, Color.yellow, 5f);

            // cast ray from camera center, if hits this object then interact with it
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
            {
                if (GameObject.ReferenceEquals(obj, hit.transform.gameObject))
                {
                    if (Input.GetButtonDown("Interact"))
                    {
                        Interact();
                        if (dialogueTrigger != null)
                        {
                            dialogueTrigger.TriggerDialogue();
                            this.enabled = false;
                        }
                        if (toggleVisAfterInteraction)
                        {
                            ToggleVisibility();
                        }
                        else if (deleteAfterInteraction)
                        {
                            Destroy(gameObject);
                        }
                        else if (deactivateAfterInteraction)
                        {
                            this.enabled = false;
                        }
                        
                    }

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

    private void ToggleVisibility()
    {
        foreach (MeshRenderer renderer in meshes)
        {
            renderer.enabled = !renderer.enabled;
        }
    }


    // check whether player is in the trigger that this script is attached to
    void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player") && !playerInTrigger)
            {
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
                playerInTrigger = false;
            }
        }
    }
}
