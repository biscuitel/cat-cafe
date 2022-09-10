using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplayer : MonoBehaviour
{
    [SerializeField] private List<Texture> images;
    [SerializeField] private float displayTime = 5.0f;
    [SerializeField] private float transitionTime = 1.0f;
    [SerializeField] private RawImage imageUI;

    private int imageIndex;
    private bool displaying;
    private float elapsed;

    // Start is called before the first frame update
    void Start()
    {
        imageIndex = 0;
        elapsed = 0.0f;
        displaying = false;
        if (!imageUI)
        {
            imageUI = this.GetComponent<RawImage>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (displaying)
        {
            elapsed += Time.deltaTime;
            // if elapsed time is greater than max display time, start transition
            if (elapsed >= displayTime)
            {
                elapsed = 0f;
                displaying = false;
                StartCoroutine(Transition());
            }
        }
    }

    void StartCutscene()
    {
        displaying = true;
    }

    IEnumerator Transition()
    {
        displaying = false;
        float transitionElapsed = 0.0f;
        float transitionMax = transitionTime / 2.0f;

        // fade out
        imageUI.CrossFadeAlpha(0f, transitionTime / 2f, false);
        while (transitionElapsed < transitionMax)
        {
            transitionElapsed += Time.deltaTime;
            
            yield return null;
        }
        
        // set UI to next cutscene image
        if (imageIndex >= images.Count - 1)
        {
            imageUI.texture = images[imageIndex++];
        }

        // fade in
        imageUI.CrossFadeAlpha(1f, transitionTime / 2f, false);
        transitionElapsed = 0.0f;
        while (transitionElapsed < transitionMax)
        {
            transitionElapsed += Time.deltaTime;
            yield return null;
        }

        // end transition
        displaying = true;
    }
}
