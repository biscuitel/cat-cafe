using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public void Update()
    {
        //if the esc key is press pause or resume the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("Pause");
        //Show the Pause Menu, Pause the Game, and allow the cursor to be used
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Resume()
    {
        Debug.Log("Resume");
        //Hide the Pause Menu, Start the Game, and hide the cursor
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        //Start the game timer and unpause the game
        Debug.Log("Main Menu");
        Time.timeScale = 1f;
        GameIsPaused = false;

        //end the game and return to the main menu
        Destroy(this.gameObject);
        SceneManager.LoadScene("Menu/Menu");

    }
}
