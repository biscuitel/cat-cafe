using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager gmInstance;

    private int levelIndex;
    private RawImage loadImage;
    [SerializeField] private List<Texture> loadImages;
    [SerializeField] private float fadeTime;
    private bool gameEnded;

    private DialogueManager dm;
    private TaskManager tm;

    void Awake()
    {
        if (gameEnded != true)
        {
            DontDestroyOnLoad(this.gameObject);
            levelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        DontDestroyOnLoad(this.gameObject);
        levelIndex = SceneManager.GetActiveScene().buildIndex;

        if (gmInstance == null)
        {
            gmInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameEnded != true)
        {
            levelIndex = 0;
            loadImage = GetComponentInChildren<RawImage>();
            loadImage.enabled = true;
            loadImage.texture = loadImages[levelIndex];
            Debug.Log(levelIndex);
            dm = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
            tm = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();
            StartCoroutine(FadeIn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Load"))
        {
            if (levelIndex < SceneManager.sceneCountInBuildSettings)
            {
                LoadNextScene();
            }
        } else if (Input.GetButtonDown("Reload"))
        {
            ReloadScene();
        } 
    }

    public void EndOfGame()
    {
        //End the game, free the cursor, show the main menu
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu/Menu");
    }

    public void LoadNextScene()
    {
        StartCoroutine(Load(false));
    }

    public void ReloadScene()
    {
        StartCoroutine(Load(true));     
    }

    IEnumerator Load(bool reload)
    {
        // fade out
        loadImage.texture = loadImages[0];
        loadImage.CrossFadeAlpha(1.0f, fadeTime, false);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // reload/load
        AsyncOperation load;
        if (reload)
        {
            load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            {
                levelIndex = 3;
            }
            else
            {
                if (levelIndex < 3)
                {
                    levelIndex++;
                }
            }
            load = SceneManager.LoadSceneAsync(levelIndex);
        }

        while(!load.isDone)
        {
            yield return null;
        }

        // fade in
        int imageIndex = levelIndex;
        loadImage.texture = loadImages[imageIndex];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);
        elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator FadeIn()
    {
       

        int imageIndex = levelIndex;
        loadImage.texture = loadImages[imageIndex];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private void GetDayImage(int sceneID)
    {
        switch (sceneID)
        {
            case 2:
                break;
            case 4:
                break;
            case 6:
                break;
            default:
                break;
        }
    }
}
