using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    private bool playerInTrigger;
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

    [SerializeField] private AudioRandomizer audioRand;
    [SerializeField] AudioSource audioSource;


    // max distance that player can interact from
    [SerializeField]  private float interactionRange = 10.0f;

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

        audioRand = GetComponent<AudioRandomizer>();
        if (audioRand)
        {
            audioSource = GetComponent<AudioSource>();
        } else
        {
            audioRand = GetComponentInChildren<AudioRandomizer>();
            audioSource = GetComponentInChildren<AudioSource>();
        }
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
                        if (Interact())
                        {
                            Debug.Log("interaction happened and completed task");
                            if (audioSource && audioSource.isPlaying)
                            {
                                audioSource.Stop();
                                Debug.Log("stopped audio");
                            }
                            if (audioRand) audioRand.PlayRandomClip();
                            if (dialogueTrigger != null)
                            {
                                dialogueTrigger.TriggerDialogue();
                                this.enabled = false;
                            }
                            if (toggleVisAfterInteraction)
                            {
                                Debug.Log("toggled vis");
                                ToggleVisibility();
                            }
                            if (deleteAfterInteraction)
                            {
                                Debug.Log("destroyed");
                                Destroy(gameObject);
                            }
                            else if (deactivateAfterInteraction)
                            {
                                Debug.Log("deactivated");
                                this.enabled = false;
                            }
                        }
                    }
                }
            }
        }
        
    }

    private bool Interact()
    {
        Debug.Log("Player interacted with me! Do a thing or smth idk");
        return taskManager.TaskComplete(taskID);

    }

    private void ToggleVisibility()
    {
        if (meshes != null)
        {
            Debug.Log("lmao");
            foreach (MeshRenderer renderer in meshes)
            {
                Debug.Log("renderer enabled = " + renderer.enabled);
                renderer.enabled = !renderer.enabled;
            }
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
