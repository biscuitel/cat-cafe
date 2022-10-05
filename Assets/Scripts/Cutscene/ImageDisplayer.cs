using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageDisplayer : MonoBehaviour
{
    [SerializeField] private List<Texture> images;
    [SerializeField] private List<float> imageDisplayTimes;
    [SerializeField] private float fadeTime = 1.0f;
    private RawImage imageUI;
    private GameManager gm;

    private int imageIndex;
    private bool displaying;
    private bool advance;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        imageIndex = 0;
        displaying = false;
        advance = true;
        if (!imageUI)
        {
            imageUI = this.GetComponentInChildren<RawImage>();
            //imageUI.CrossFadeAlpha(0, 0, false);
        }
        Debug.Log("image count = " + images.Count);
        StartCutscene();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            advance = true;
        }
    }

    void StartCutscene()
    {
        Debug.Log("Started cutscene");
        displaying = true;
        imageUI.texture = images[imageIndex];
        //imageUI.SetNativeSize();
        StartCoroutine(FadeIn());
    }

    void nextImage()
    {
        // set UI to next cutscene image
        if (imageIndex < images.Count - 1)
        {
            imageIndex++;
            Debug.Log("swapping to image " + imageIndex);
            imageUI.texture = images[imageIndex];
            //imageUI.SetNativeSize();
            StartCoroutine(ImageTimer());
        } else
        {
            Debug.Log("Fading out");
            StartCoroutine(FadeOut());
        }
        
    }

    IEnumerator FadeIn()
    {
        //Debug.Log("Fading in");
        float elapsed = 0.0f;
        //imageUI.CrossFadeAlpha(1, fadeTime, false);
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }
        StartCoroutine(ImageTimer());
    }

    IEnumerator FadeOut()
    {
        while (!advance)
        {
            yield return null;
        }

        float elapsed = 0.0f;
        //imageUI.CrossFadeAlpha(0, fadeTime, false);
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }
        // load next scene or w/e
        gm.LoadNextScene();
    }

    IEnumerator ImageTimer()
    {
        float elapsed = 0.0f;

        // fade out
        while (elapsed < imageDisplayTimes[imageIndex])
        {
            elapsed += Time.deltaTime;
            
            yield return null;
        }

        nextImage();
    }
}
