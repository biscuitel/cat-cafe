using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager gmInstance;

    [HideInInspector] public int levelIndex;
    private RawImage loadImage;
    [SerializeField] private List<Texture> loadImages;
    [SerializeField] private float fadeTime;
    private bool gameEnded;
    private AudioSource BGM;

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
            levelIndex = SceneManager.GetActiveScene().buildIndex;
            loadImage = GetComponentInChildren<RawImage>();
            loadImage.enabled = true;
            loadImage.texture = loadImages[GetDayImage(levelIndex)];
            Debug.Log(levelIndex);
            StartCoroutine(FadeIn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
        
    }

    public void EndOfGame()
    {
        //End the game, free the cursor, show the main menu
        Debug.Log("game has been ended");
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(FadeOutToMain());
    }

    public void LoadNextScene()
    {
        StartCoroutine(Load(false));
    }

    public void ReloadScene()
    {
        StartCoroutine(Load(true));     
    }

    public void LoadFirst()
    {
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator Load(bool reload)
    {
        // fade out
        loadImage.texture = loadImages[0];
        loadImage.CrossFadeAlpha(1.0f, fadeTime, false);
        float elapsed = 0f;

        StartCoroutine(BGMStartFadeOut());

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
            if (levelIndex >= SceneManager.sceneCountInBuildSettings - 1)
            {
                levelIndex = 0;
                load = SceneManager.LoadSceneAsync(0);
            }
            else
            {
                if (levelIndex < SceneManager.sceneCountInBuildSettings - 1)
                {
                    levelIndex++;
                }
            }
            load = SceneManager.LoadSceneAsync(levelIndex);
        }
        Debug.Log(levelIndex);

        while(!load.isDone)
        {
            yield return null;
        }

        // fade in
        int imageIndex = levelIndex;
        loadImage.texture = loadImages[GetDayImage(imageIndex)];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);


        StartCoroutine(BGMStartFadeIn());

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
        loadImage.texture = loadImages[GetDayImage(imageIndex)];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator BGMStartFadeIn()
    {
        if (GameObject.FindGameObjectWithTag("BGM"))
        {
            BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
            float targetVolume = BGM.volume;
            BGM.volume = 0;
            float currentTime = 0;
            float start = BGM.volume;
            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                if (BGM)
                {
                    BGM.volume = Mathf.Lerp(start, targetVolume, currentTime / fadeTime);
                }
                
                yield return null;
            }
            yield break;
        }
    }

    IEnumerator BGMStartFadeOut()
    {
        if (GameObject.FindGameObjectWithTag("BGM"))
        {
            BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();

            float currentTime = 0;
            float start = BGM.volume;
            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                BGM.volume = Mathf.Lerp(start, 0, currentTime / fadeTime);
                yield return null;
            }
            yield break;
        }
    }


    public void StartFadeIn()
    {
        StartCoroutine(FadeOutToMain());
    }

    IEnumerator FadeOutToMain()
    {
        loadImage.texture = loadImages[0];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadSceneAsync(0);

    }

    public void LoadMainMenu()
    {
        levelIndex = 0;
        StartCoroutine(FadeOutToMain());
    }

    private int GetDayImage(int sceneID)
    {
        switch (sceneID)
        {
            case 2:
                return 1;
                break;
            case 4:
                return 2;
                break;
            case 6:
                return 3;
                break;
            default:
                return 0;
                break;
        }
    }
}
