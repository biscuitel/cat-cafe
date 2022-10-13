using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "DialogueObjects/Dialogue", order = 1)]
public class DialogueSO : ScriptableObject
{
    public enum Speaker
    {
        Player,
        Boss
    }


    [SerializeField] [TextArea] private List<string> dialogue;
    [SerializeField] private List<Speaker> characterSpeaking;
    [SerializeField] private bool onPhone = true;
    [SerializeField] public List<Texture> characterPortrait;

    public List<string> GetDialogue()
    {
        return dialogue;
    }

    public List<Speaker> GetSpeaker()
    {
        return characterSpeaking;
    }

    public bool isOnPhone()
    {
        return onPhone;
    }
    
    public List<Texture> GetPortrait()
    {
        return characterPortrait; 
    }
    
}
