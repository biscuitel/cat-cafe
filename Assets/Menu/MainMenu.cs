using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //set the timer to be normal time and load the Day 1 scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("Days/Day 1/Day 1");
    }

}
