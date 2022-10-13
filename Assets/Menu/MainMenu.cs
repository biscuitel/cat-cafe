using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gm;
    private bool show = true;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void StartGame()
    {
        //set the timer to be normal time and load the Day 1 scene
        Time.timeScale = 1f;
        gm.LoadNextScene();
        //SceneManager.LoadScene("Days/Day 1/Day 1");
    }

    public void ShowInstructions(GameObject InstructionMenu)
    {
        //show or don't show the Instruction Menu
        InstructionMenu.SetActive(show);
        //switch the boolean to indicate Instruction Menu is visible
        show = !show;
    }
}
