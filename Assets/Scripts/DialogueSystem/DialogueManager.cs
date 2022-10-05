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
    public GameObject DialogueUI;
    

    [SerializeField] private List<DialogueSO> dialogue;
    [SerializeField] private AudioClip phonePickUp;
    [SerializeField] private AudioClip phoneHangUp;
    [SerializeField] private AudioSource phoneAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        //DialogueUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetButtonDown("Interact"))
        {
            DisplayDialogue();
        }
        
    }

    private void DisplayDialogue()
    {
        if (dialogueIndex < dialogue[listIndex].GetDialogue().Count)
        {
            Debug.Log(dialogueIndex);
            //show the dialog canvas
            ShowDialog(true);
            UpdateUI();
            dialogueIndex++;
        } else
        {
            //hide the dialog canvas
            ShowDialog(false);
            dialogueIndex = 0;
            dialogueText.text = "";
            dialogueActive = false;
            if (dialogue[listIndex].isOnPhone() && phoneAudioSource)
            {
                phoneAudioSource.volume = 0.75f;
                phoneAudioSource.clip = phoneHangUp;
                phoneAudioSource.Play();
            }
            Debug.Log("dialogue deactivated");
        }
    }

    void UpdateUI()
    {
        Debug.Log(dialogue[listIndex].GetDialogue()[dialogueIndex]);
        dialogueText.text = dialogue[listIndex].GetDialogue()[dialogueIndex];
    }

    public void ActivateDialogue(int index)
    {
        if (dialogue[listIndex].isOnPhone() && phoneAudioSource)
        {
            phoneAudioSource.volume = 0.75f;
            phoneAudioSource.loop = false;
            phoneAudioSource.clip = phonePickUp;
            phoneAudioSource.Play();
        }
        dialogueActive = true;
        if (index >= 0 && index < dialogue.Count)
        {
            listIndex = index;
        }
        DisplayDialogue();
    }

    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    public void ShowDialog(bool show)
    {
        DialogueUI.SetActive(show);
        // also set the children of this object to be the same active status
        foreach (Transform t in DialogueUI.transform)
        {
            t.gameObject.SetActive(show); 
        }
    }
}
