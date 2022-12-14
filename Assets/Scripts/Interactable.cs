using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{

    private bool playerInTrigger;
    private int raycastLayerMask;
    private Camera cam;
    private GameObject obj;
    private TaskManager taskManager;
    private DialogueTrigger dialogueTrigger;
    private bool interactedWith;
    private Graphic interactionCrosshair;
    private Animator taskCompleteAnimation;

    [SerializeField] private int taskID;
    [SerializeField] private bool deactivateAfterInteraction = false;
    [SerializeField] private bool deleteAfterInteraction = false;
    [SerializeField] private float destroyAfter = 0f;
    [SerializeField] private bool toggleVisAfterInteraction = false;

    [SerializeField] private MeshRenderer[] meshes;

    [SerializeField] private AudioRandomizer audioRand;
    [SerializeField] AudioSource audioSource;


    private void Awake()
    {
        taskCompleteAnimation = GameObject.FindGameObjectWithTag("TaskCompleteAnimator").GetComponent<Animator>();

    }
    void Start()
    {
        raycastLayerMask = 1 << this.gameObject.layer;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInTrigger = false;
        obj = this.transform.GetChild(0).gameObject;
        taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();
        dialogueTrigger = this.GetComponent<DialogueTrigger>();
        meshes = this.GetComponentsInChildren<MeshRenderer>();
        interactedWith = false;
        interactionCrosshair = GameObject.FindGameObjectWithTag("InteractionCrosshair").GetComponent<Graphic>();

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
      
      // get camera center
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // draw line of raycast for debugging purposes
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1.7f, Color.yellow, 5f);

            // cast ray from camera center, if hits this object then interact with it
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.7f, raycastLayerMask, QueryTriggerInteraction.Collide))
            {
                if (GameObject.ReferenceEquals(obj, hit.transform.gameObject))
                {
                    if (taskManager.CheckForTask(taskID))
                    {
                        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 1);
                    }
         
                    if (Input.GetButtonDown("Interact") && !interactedWith)
                    {                       
                        if (Interact())
                        {


                        if (taskCompleteAnimation)
                        {
                            taskCompleteAnimation.Play("TaskComplete");
                        }

                        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);
                            interactedWith = true;
                            Debug.Log("interaction happened and completed task");
                            if (audioSource && audioSource.isPlaying)
                            {
                                audioSource.Stop();
                                Debug.Log("stopped audio");
                            }
                            if (audioRand && audioSource) audioRand.PlayRandomClip();
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
                                StartCoroutine(DelayDestroy(destroyAfter));
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
        else
        {
            interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);

        }
        

    }

    private bool Interact()
    {
        Debug.Log("Player interacted with me! Do a thing or smth idk");
        return taskManager.TaskComplete(taskID);

    }

    IEnumerator DelayDestroy(float delayTime)
    {
        float elapsed = 0f;
        while (elapsed < delayTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
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
