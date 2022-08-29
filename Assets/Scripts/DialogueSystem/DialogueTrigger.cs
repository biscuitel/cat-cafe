using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private int dialogueIndex = -1;

    private void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    void Update()
    {

    }

    public void TriggerDialogue()
    {
        Debug.Log("Dialogue triggered with index = " + 0);
        dialogueManager.ActivateDialogue(dialogueIndex);
    }
}
