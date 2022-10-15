using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1Outcomes : Outcomes
{
    private GameManager gm;
    private TaskManager tm;
    private DialogueManager dm;
    private MedsEffects cameraEffects;

    private int poopCounter = 0;
    private int cupCounter = 0;

    [SerializeField] private GameObject taskUI;

    [Header("Objects")]
    [SerializeField] private Renderer TaskList;

    [Header("Animators")]
    [SerializeField] private Animator signAnim;
    [SerializeField] private Animator binAnim;
    [SerializeField] private Animator phoneAnim;
    [SerializeField] private Animator switchAnim;
    private Animator vacuumAnimator;
    private Animator scoopAnimator;

    [Header("Audio")]
    [SerializeField] private GameObject PhonePickup;
    // The items that will appear in the players hand. They are seperate to the object the player interacts with to begin the task
    // Instead they are attached to the camera or player (depending on whether or not they should follow where the player looks)
    [Header("Hand Items")]
    [SerializeField] private GameObject Vacuum;
    [SerializeField] private GameObject Scooper;
    [SerializeField] private GameObject Cup1;
    [SerializeField] private GameObject Cup2;
    [SerializeField] private GameObject Cup3;
    [SerializeField] private GameObject Poop1;
    [SerializeField] private GameObject Poop2;
    [SerializeField] private GameObject Poop3;
    [SerializeField] private GameObject PoopParent;
    [SerializeField] private GameObject CupsParent;

    //These are the objects that the player interacts with the start the task, e.g. the vacuum in the storage room
    //I'm using these variables for the purpose of changing their material to an outline shader
    [Header("Base Items")]
    [SerializeField] private GameObject BaseVacuum;
    [SerializeField] private GameObject BaseScooper;
    [SerializeField] private GameObject AntihistamineBox;

    [Header("Placed Items")]

    [SerializeField] private GameObject PlacedMugs;

    //Since each object needs to change materials, I'm storing their original materials here, as well as the outline material.
    [Header("Materials")]
    [SerializeField] private Material OutlineMat;

    [SerializeField] private Material VacuumMat;
    [SerializeField] private Material ScooperMat;
    [SerializeField] private Material CupMat;

    [Header("Task Board Materials")]
    [SerializeField] private Material Taskboard1stTaskDoneMat;
    [SerializeField] private Material Taskboard2ndTaskDoneMat;
    [SerializeField] private Material Taskboard3rdTaskDoneMat;
    [SerializeField] private Material Taskboard4thTaskDoneMat;

    private AudioSource bgm;
    [SerializeField] private float musicTargetPitch = 1f;
    [SerializeField] private float pitchSlideTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        tm = GetComponent<TaskManager>();
        cameraEffects = GetComponent<MedsEffects>();

        vacuumAnimator = Vacuum.GetComponent<Animator>();
        scoopAnimator = Scooper.GetComponent<Animator>();
        taskUI.SetActive(false);

        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        if (bgm) StartCoroutine(PitchSlide(bgm, musicTargetPitch, pitchSlideTime));

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
                if (phoneAnim)
                {
                    //phoneAnim.SetBool("StartAnimation", true);
                    phoneAnim.SetTrigger("phoneStop");
                }


                PhonePickup.gameObject.GetComponent<AudioSource>().Play();
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
                
                

                // player allergies begin to react - do thing here
                cameraEffects.StartDistort();
                cameraEffects.TimeTrigger();

                if (cameraEffects.HasMeds())
                {
                    tm.ActivateTask(3);
                    cameraEffects.StartPromptTimer();
                } 
                else
                {
                    tm.ActivateTask(2);
                }

                tm.DeactivateTask(100);
                break;
                //20 is the task that is always active in the background so that the player can pick up the antihistmines at any time before the task telling them to do so.
            case 100:
                cameraEffects.SetHasMeds(true);
                AntihistamineBox.SetActive(false);
                break;
            case 2:
                // player grabbed antihistamenes for their allergies
                cameraEffects.SetHasMeds(true);
                cameraEffects.StartPromptTimer();

                AntihistamineBox.SetActive(false);

                cameraEffects.ActivatePrompt();
                // revert effects and activate next task
                tm.ActivateTask(3);
                break;
            case 3:
                // player interacted with task board; populate task list
                tm.ActivateTask(4);
                TaskList.material = Taskboard1stTaskDoneMat;
                break;
            case 4:
                // player grabs vacuum cleaner, populate list with hair cleaning tasks
                tm.ActivateGroup(0);

                //turns on vaccuum in players hand
                Vacuum.SetActive(true);

                //turns the base vacuum off by going through each childs meshes are disabling the renderer
                break;
            case 5:
                // player cleaned (all) cat hair
                // activate task to return vacuum cleaner
                tm.ActivateTask(5);

                //turns the vacuum back on with the outline shader
                foreach (MeshRenderer renderer in BaseVacuum.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                    renderer.material = OutlineMat;
                }

                break;
            case 6:
                // prompt player to check taskboard again
                tm.ActivateTask(6);
                
                //turns off vacuum in players hand
                Vacuum.SetActive(false);

                foreach (MeshRenderer renderer in BaseVacuum.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.material = VacuumMat;
                }
                break;
            case 7:
                // prompt player to grab poop scooper
                tm.ActivateTask(7);
                TaskList.material = Taskboard2ndTaskDoneMat;
                break;
            case 8:
                //player picked up scooper
                //activate tasks to scoop all the poop              
                tm.ActivateGroup(1);

                //turns on scooper in players hand
                Scooper.SetActive(true);

                

                break;
            
            case 9:
                // player scoops (all) poop
                //player prompted to return scooper
                tm.ActivateTask(8);

               

                break;
            //the reason this is here is because placing a new task in the middle of the already created tasks is a long and arduous process
            //so I didn't feel like moving every single task up or down a digit in the outcome IDs - Xavier
            case -10:
                if (binAnim)
                {
                    //signAnim.SetBool("StartAnimation", true);
                    binAnim.SetTrigger("LidSpin");
                }

                Debug.Log("The lid spins.");
                PoopParent.SetActive(false);
                tm.ActivateTask(9);

                 foreach (MeshRenderer renderer in BaseScooper.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                    renderer.material = OutlineMat;
                }
                break;


            case 10:
                // prompt player to check taskboard for 3rd time
                tm.ActivateTask(10);

                //turns off scooper in players hand
                Scooper.SetActive(false);

                foreach (MeshRenderer renderer in BaseScooper.GetComponentsInChildren<MeshRenderer>())
                {
                    
                    renderer.material = ScooperMat;
                }

                
                break;

            case 11:
                //prompt player to collect dishes
                tm.ActivateGroup(2);
                TaskList.material = Taskboard3rdTaskDoneMat;
                break;
            case 12:
                //player collected dishes
                //tell player to put dishes into sink
                tm.ActivateTask(11);


                //Turns on the outline of where the mugs should be placed
                foreach (MeshRenderer renderer in PlacedMugs.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                    renderer.material = OutlineMat;
                }

                break;
            case 13:
                //turn light switch off
                tm.ActivateTask(12);

                //once mugs are placed, change their material back to normal
                foreach (MeshRenderer renderer in PlacedMugs.GetComponentsInChildren<MeshRenderer>())
                {                    
                    renderer.material = CupMat;
                }
                
                CupsParent.SetActive(false);
                break;
            case 14:
                if (switchAnim)
                {
                    //switchAnim.SetBool("StartAnimation", true);
                    switchAnim.SetTrigger("switchoff");
                }
                // player completed all task for the day, level end

                Destroy(taskUI);
                gm.LoadNextScene();
                break;

            case -1:

                vacuumAnimator.Play("VacuumSlideDay1");

                break;
            case -2:
                // player completed all task for the day, level end
                scoopAnimator.Play("Day1Scoop");
                poopCounter++;

                if (poopCounter == 1)
                {
                    Poop1.SetActive(true);
                }
                else if (poopCounter == 2)
                {
                    Poop2.SetActive(true);
                }
                else if (poopCounter == 3)
                {
                    Poop3.SetActive(true);
                }

                break;
            case -3:
                cupCounter++;

                if (cupCounter == 1)
                {
                    Cup1.SetActive(true);
                }
                else if (cupCounter == 2)
                {
                    Cup2.SetActive(true);
                }
                else if (cupCounter == 3)
                {
                    Cup3.SetActive(true);
                }

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
