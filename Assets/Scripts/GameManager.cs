using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int levelIndex;
    private RawImage loadImage;
    [SerializeField] private List<Texture> loadImages;
    [SerializeField] private float fadeTime;

    private void Awake()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        loadImage = GetComponentInChildren<RawImage>();
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Load"))
        {
            LoadNextScene();
        } else if (Input.GetButtonDown("Reload"))
        {
            ReloadScene();
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(Load(false));
    }

    public void ReloadScene()
    {
        StartCoroutine(Load(true));
        //StartCoroutine(FadeOut());
        //StartCoroutine(FadeIn(load));
        
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
        } else
        {
            if (levelIndex >= SceneManager.sceneCountInBuildSettings - 1)
            {
                levelIndex = 0;
            }
            else
            {
                levelIndex++;
            }
            load = SceneManager.LoadSceneAsync(levelIndex);
        }

        // fade in
        int imageIndex = levelIndex + 1 > SceneManager.sceneCountInBuildSettings ? levelIndex = 0 : levelIndex + 1;
        Debug.Log(imageIndex);
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
        int imageIndex = levelIndex + 1 > SceneManager.sceneCountInBuildSettings ? levelIndex = 0 : levelIndex + 1;
        Debug.Log(imageIndex);
        loadImage.texture = loadImages[imageIndex];
        loadImage.CrossFadeAlpha(0f, fadeTime, false);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
