using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverIconChanger : MonoBehaviour
{
    private int raycastLayerMask;
    private float interactionRange = 1.5f;
    private Camera cam;
    
    private Graphic interactionCrosshair;
    
    void Start()
    {
        raycastLayerMask = LayerMask.GetMask("ClickableObject");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        interactionCrosshair = GameObject.FindGameObjectWithTag("InteractionCrosshair").GetComponent<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        IconChange();
    }
    void IconChange()
    { 
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, raycastLayerMask, QueryTriggerInteraction.Collide))
                {         
                        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 1);
                }
                else
                {
                    interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);
                }

    }
}