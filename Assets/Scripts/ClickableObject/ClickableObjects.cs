using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjects : MonoBehaviour
{
    private Animator InteractionObject;
    private int raycastLayerMask;
    [SerializeField] private float interactionRange = 10.0f;
    private Camera cam;
    private GameObject obj;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        raycastLayerMask = 1 << this.gameObject.layer;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        obj = this.gameObject;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        InteractionCheck();

    }

    void InteractionCheck()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
        {
            if (GameObject.ReferenceEquals(obj, hit.transform.gameObject))
            {
                if (Input.GetButtonDown("Interact"))
                {
                    
                    Debug.Log("FFESfsefsnefoiuseuibvf");
                }
            }
        }


    }
}
