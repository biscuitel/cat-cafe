using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Outcomes : Outcomes
{

    private GameManager gm;
    private TaskManager tm;
    private DialogueManager dm;
    private MedsEffects cameraEffects;
    [SerializeField] private Animator signAnim;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        tm = GetComponent<TaskManager>();
        cameraEffects = GetComponent<MedsEffects>();
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
                if (signAnim)
                {
                    //signAnim.SetBool("StartAnimation", true);
                    signAnim.SetTrigger("TriggerAnimation");
                }
                // activate task board task
                tm.ActivateTask(2);
                // flip sign model here

                // player allergies begin to react - do thing here
                cameraEffects.StartDistort();
                break;
            case 2:
                // player took antihistamenes for their allergies
                // revert effects and activate next task
                cameraEffects.StartUndistort();
                tm.ActivateTask(3);
                break;
            case 3:
                // player interacted with task board; populate task list
                tm.ActivateTask(4);
                break;
            case 4:
                // player grabs vacuum cleaner, populate list with hair cleaning tasks
                tm.ActivateGroup(0);
                break;
            case 5:
                // player cleaned (all) cat hair
                // activate task to return vacuum cleaner
                tm.ActivateTask(5);
                break;
            case 6:
                // prompt player to check taskboard again
                tm.ActivateTask(6);
                break;
            case 7:
                // prompt player to grab poop scooper
                tm.ActivateTask(7);
                break;
            case 8:
                //player picked up scooper
                //activate tasks to scoop all the poop
                tm.ActivateGroup(1);
                break;
            case 9:
                // player scoops (all) poop
                //player prompted to return scooper
                tm.ActivateTask(8);
                break;
            case 10:
                // prompt player to check taskboard for 3rd time
                tm.ActivateTask(9);
                break;
            case 11:
                //prompt player to collect dishes
                tm.ActivateGroup(2);
                break;
            case 12:
                //player collected dishes
                //tell player to put dishes into sink
                tm.ActivateTask(10);
                break;
            case 13:
                // player completed all task for the day, level end
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
