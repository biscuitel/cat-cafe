using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverIconChanger : MonoBehaviour
{
    private int clickableRaycastLayerMask;
    private float interactionRange = 1.5f;
    private Camera cam;
    
    private Graphic interactionCrosshair;
   
    void Start()
    {
        clickableRaycastLayerMask = LayerMask.GetMask("ClickableObject");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        interactionCrosshair = GameObject.FindGameObjectWithTag("ClickableCrosshair").GetComponent<Graphic>();
 
    }

    // Update is called once per frame
    void Update()
    {
        ClickableIconChange();
        
    }
    void ClickableIconChange()
    { 
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, clickableRaycastLayerMask, QueryTriggerInteraction.Collide))
                {         
                        interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 1);
                }
                else
                {
                    interactionCrosshair.color = new Color(interactionCrosshair.color.r, interactionCrosshair.color.g, interactionCrosshair.color.b, 0);
                }
    }
}