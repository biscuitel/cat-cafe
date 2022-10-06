 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClickableObject : MonoBehaviour
{


    private int raycastLayerMask;
    [SerializeField] private float interactionRange = 10.0f;
    private Camera cam;
    private Vector3 camPos;
    private AudioSource audioSource;
    private Graphic interactionCrosshair;

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


        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);



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
            if (Vector3.Distance(this.transform.position, camPos) < 2)
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
                {
                    interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 1);

                    if (Input.GetButtonDown("Interact"))
                    {
                        Debug.Log("Clickable object CLICKED!!!?????");
                        if (audioSource) audioSource.Play();
                        hit.transform.GetComponent<Animator>().SetTrigger("Interaction");

                    }
                }
                else
                {
                    interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);

                }
            }


        }
    }
}
