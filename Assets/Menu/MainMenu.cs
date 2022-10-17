using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameManager gm;
    private Graphic credits;
    private Graphic controls;


    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        credits = GameObject.FindGameObjectWithTag("Credits").GetComponent<Graphic>();
        controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<Graphic>();

        controls.enabled = false;
        credits.enabled = false;
    }

    public void StartGame()
    {
        //set the timer to be normal time and load the Day 1 scene
        Time.timeScale = 1f;
        gm.LoadNextScene();
        //SceneManager.LoadScene("Days/Day 1/Day 1");

        
    }

    public void ShowInstructions(Graphic InstructionMenu)
    {

        credits.enabled = false;

        if (InstructionMenu.enabled)
        {
            InstructionMenu.enabled = false;    
        } 
        else
        {
            InstructionMenu.enabled = true;
        }
        //show or don't show the Instruction Menu
        
        //switch the boolean to indicate Instruction Menu is visible
        
    }

    public void ShowCredits(Graphic Credits)
    {

        controls.enabled = false;

        if (Credits.enabled)
        {
            Credits.enabled = false;
        }
        else
        {
            Credits.enabled = true;
        }

    }

}
