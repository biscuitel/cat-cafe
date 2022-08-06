using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{

    private bool playerInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        playerInTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger && Input.GetButtonDown("Interact")){
            Interact();
        }
    }

    public void Interact()
    {
        Debug.Log("Player interacted with me! Do a thing or smth idk");
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
