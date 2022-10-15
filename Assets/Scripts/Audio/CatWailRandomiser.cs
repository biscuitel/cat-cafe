using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWailRandomiser : MonoBehaviour
{


    private AudioRandomizer aR;
    private AudioSource aS;

    void Start()
    {
        aR = gameObject.GetComponent<AudioRandomizer>();
        aS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        playRandomNoise();
    }

    void playRandomNoise()
    {

        if (!aS.isPlaying)
        {
            aR.PlayRandomClip();
        }

    }

}
