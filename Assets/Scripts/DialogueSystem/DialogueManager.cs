using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private bool dialogueActive = false;
    private int dialogueIndex = 0;
    public Text dialogueText;

    [SerializeField] private DialogueSO dialogue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive)
        {
            DisplayDialogue();
        }
        
    }

    private void DisplayDialogue()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (dialogueIndex < dialogue.GetDialogue().Count)
            {
                Debug.Log(dialogueIndex);
                UpdateUI();
                dialogueIndex++;
            } else
            {
                dialogueIndex = 0;
                dialogueText.text = "";
                dialogueActive = false;
            }
        }
    }

    void UpdateUI()
    {
        // dialogueUI.characterSpeaking = dialogue.GetSpeaker()[dialogueIndex];
        Debug.Log(dialogue.GetDialogue()[dialogueIndex]);
        dialogueText.text = dialogue.GetDialogue()[dialogueIndex];
    }

    public void ActivateDialogue()
    {
        dialogueActive = true;
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }
}
