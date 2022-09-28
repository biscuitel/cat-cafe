using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{

    
    private int raycastLayerMask;
    [SerializeField] private float interactionRange = 10.0f;
    private Camera cam;


    
    // Start is called before the first frame update
    void Start()
    {

        
        raycastLayerMask = LayerMask.GetMask("ClickableObject");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {

        InteractionCheck();

    }

    void InteractionCheck()
    {
    
        if (Input.GetButtonDown("Interact"))
        {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
            {
                
                Debug.Log("Clickable object CLICKED!!!?????");
                
                hit.transform.GetComponent<Animator>().Play("Interaction");

            }   
        }

    }
}
