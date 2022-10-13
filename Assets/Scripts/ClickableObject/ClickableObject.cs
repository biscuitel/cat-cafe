 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClickableObject : MonoBehaviour
{


    private int raycastLayerMask;
    private float interactionRange = 1.5f;
    private Camera cam;
    private Vector3 camPos;
    private AudioSource audioSource;
    private AudioRandomizer randomiser;
    private Graphic interactionCrosshair;
    private GameObject obj;
    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        raycastLayerMask = LayerMask.GetMask("ClickableObject");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        interactionCrosshair = GameObject.FindGameObjectWithTag("InteractionCrosshair").GetComponent<Graphic>();

        if (GetComponent<AudioRandomizer>())
        {
            randomiser = GetComponent<AudioRandomizer>();
        }
        
        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);

        obj = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        InteractionCheck();

    }

    void InteractionCheck()
    {
        if (GetComponent<Renderer>().isVisible)
            camPos = cam.gameObject.transform.position;

        {
            if (Vector3.Distance(this.transform.position, camPos) < 3)
            {

                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
                {
                    if (GameObject.ReferenceEquals(obj, hit.transform.gameObject))
                    {                     
                        if (Input.GetButtonDown("Interact"))
                        {
                            Debug.Log("Clickable object CLICKED!!!?????");


                            if (!audioSource.isPlaying)
                            {

                                if (randomiser)
                                {
                                    randomiser.PlayRandomClip();
                                }
                                else
                                {
                                    if (audioSource) audioSource.Play();
                                }
                            }

                            

                            if (GetComponent<Animator>()) hit.transform.GetComponent<Animator>().SetTrigger("Interaction");

                            if (particles) particles.Play();

                        }
                    } 
                }
            }
        }    
    }
}
