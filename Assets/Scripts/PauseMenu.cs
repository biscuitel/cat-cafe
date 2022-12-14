using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private GameManager gm;

    private AudioSource walkSound;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        walkSound = GameObject.FindGameObjectWithTag("WalkSound").GetComponent<AudioSource>();
    }

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

        walkSound.Pause();
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
        gm.levelIndex = 0;
        Debug.Log("Main Menu");
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
    }
}
