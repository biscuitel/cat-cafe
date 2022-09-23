﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day3Outcomes : Outcomes
{

    private GameManager gm;
    private TaskManager tm;
    private DialogueManager dm;
    private MedsEffects cameraEffects;
    [SerializeField] private Animator signAnim;
    [SerializeField] private Animator binAnim;
    [SerializeField] private Animator phone3Anim;

    [SerializeField] private GameObject warpTrigger;

    [SerializeField] private GameObject BigButton;
    [SerializeField] private GameObject CatCagesParent;


    //the items that will appear in the players hand. They are seperate to the object the player interacts with to begin the task
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


    //These are the objects that the player interacts with the start the task, e.g. the vacuum in the storage room
    //I'm using these variables for the purpose of changing their material to an outline shader
    [Header("Base Items")]
    [SerializeField] private GameObject BaseVacuum;
    [SerializeField] private GameObject BaseScooper;

    //Since each object needs to change materials, I'm storing their original materials here, as well as the outline material.
    [Header("Materials")]
    [SerializeField] private Material OutlineMat;

    [SerializeField] private Material VacuumMat;
    [SerializeField] private Material ScooperMat;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        tm = GetComponent<TaskManager>();
        cameraEffects = GetComponent<MedsEffects>();
        // have allergies use time instead of manual trigger by default
        cameraEffects.TimeTrigger();
        warpTrigger.SetActive(false);
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
                if (phone3Anim)
                {
                    //phoneAnim.SetBool("StartAnimation", true);
                    phone3Anim.SetTrigger("phoneoff");
                }
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
                break;
            case 2:
                // player grabbed antihistamenes for their allergies
                cameraEffects.SetHasMeds(true);
                cameraEffects.StartPromptTimer();
                // revert effects and activate next task
                tm.ActivateTask(3);
                break;
            case 3:
                // player interacted with task board; populate task list
                tm.ActivateTask(4);
                break;
            case 4:
                // player grabs vacuum cleaner, populate list with hair cleaning tasks
                tm.ActivateGroup(0);

                //turns on vaccuum in players hand
                Vacuum.SetActive(true);

                //turns the base vacuum off by going through each childs meshes are disabling the renderer
                foreach (MeshRenderer renderer in BaseVacuum.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = false;

                }
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
                    renderer.enabled = true;
                    renderer.material = VacuumMat;
                }
                break;
            case 7:
                // prompt player to grab poop scooper
                tm.ActivateTask(7);
                break;
            case 8:
                //player picked up scooper
                //activate tasks to scoop all the poop              
                tm.ActivateGroup(1);

                //turns on scooper in players hand
                Scooper.SetActive(true);

                foreach (MeshRenderer renderer in BaseScooper.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = false;

                }

                break;
            case 9:
                // player scoops (all) poop
                //player prompted to return scooper
                if (binAnim)
                {
                    //phoneAnim.SetBool("StartAnimation", true);
                    binAnim.SetTrigger("LidSpin");
                }
                tm.ActivateTask(8);

                foreach (MeshRenderer renderer in BaseScooper.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                    renderer.material = OutlineMat;
                }

                break;

            case 10:
                // prompt player to check taskboard for 3rd time
                tm.ActivateTask(9);
                
                //turns off scooper in players hand
                Scooper.SetActive(false);

                foreach (MeshRenderer renderer in BaseScooper.GetComponentsInChildren<MeshRenderer>())
                {
                    renderer.enabled = true;
                    renderer.material = ScooperMat;
                }

                break;
            case 11:
                //prompt player to collect dishes
                tm.ActivateTask(13);
                warpTrigger.SetActive(true);
                break;
           
            case 14:
                // prompt player to enter staff room, activate warp to final corridor
                warpTrigger.SetActive(true);
                Debug.Log("player was teleported to end corridor");
                tm.ActivateTask(12);
                break;
            case 15:
                //prompt player to interact with phone at end of corridor
                tm.ActivateTask(13);
                break;
            case 16:
                // player interacted with phone at end of corridor, make button appear!
                tm.ActivateTask(14);
                BigButton.SetActive(true);
                break;
            case 17:
                // player pressed the button, open the cages

                foreach (Animator animator in CatCagesParent.GetComponentsInChildren<Animator>())
                {
                    animator.SetBool("Open", true);
                }
                gm.LoadNextScene();
                // player interacted with phone at end of corridor, do thing
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

