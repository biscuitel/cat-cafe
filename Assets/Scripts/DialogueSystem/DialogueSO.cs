using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<string> GetDialogue()
    {
        return dialogue;
    }

    public List<Speaker> GetSpeaker()
    {
        return characterSpeaking;
    }
}
