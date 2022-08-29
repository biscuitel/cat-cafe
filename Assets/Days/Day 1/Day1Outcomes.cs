using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Outcomes : Outcomes
{

    private GameManager gm;
    private TaskManager tm;
    private DialogueManager dm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        tm = GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Outcome(int outcomeID)
    {
        // put outcome switch here
        switch (outcomeID)
        {
            case 0:
                // player interacted with phone, initial conversation on day 1 is handled by dialoguemanager
                // activate next task
                tm.ActivateTask(1);
                break;
            case 1:
                // player flipped sign to "closed" on front door
                // activate task board task
                tm.ActivateTask(2);
                // flip sign model here
                break;
            case 2:
                // TODO - player's allergies begin to react
                // player interacted with task board; populate task list
                tm.ActivateTask(3);
                break;
            case 3:
                // player grabs vacuum cleaner, populate list with hair cleaning tasks
                tm.ActivateGroup(0);
                break;
            case 4:
                // player cleaned (all) cat hair
                // activate task to return vacuum cleaner
                tm.ActivateTask(4);
                break;
            case 5:
                // player returned vacuum cleaner to storage room, level end
                gm.LoadNextScene();
                break;
            default:
                break;
        }
    }

    IEnumerator PlayerInDialogue()
    {
        while (dm.IsDialogueActive())
        {
            yield return null;
        }
        gm.StartFadeIn();
    }
}
