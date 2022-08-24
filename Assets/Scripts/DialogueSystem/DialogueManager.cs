using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private bool dialogueActive = false;
    private int dialogueIndex = 0;
    private int listIndex = 0;
    public Text dialogueText;
    

    [SerializeField] private List<DialogueSO> dialogue;

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
            if (dialogueIndex < dialogue[listIndex].GetDialogue().Count)
            {
                Debug.Log(dialogueIndex);
                UpdateUI();
                dialogueIndex++;
            } else
            {
                dialogueIndex = 0;
                dialogueText.text = "";
                dialogueActive = false;
                Debug.Log("dialogue deactivated");
            }
        }
    }

    void UpdateUI()
    {
        // dialogueUI.characterSpeaking = dialogue.GetSpeaker()[dialogueIndex];
        Debug.Log(dialogue[listIndex].GetDialogue()[dialogueIndex]);
        dialogueText.text = dialogue[listIndex].GetDialogue()[dialogueIndex];
    }

    public void ActivateDialogue(int index)
    {
        dialogueActive = true;
        if (index >= 0 && index < dialogue.Count)
        {
            listIndex = index;
        }
        
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }
}
