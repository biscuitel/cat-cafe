using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntro : MonoBehaviour
{

    private AudioSource intro;
    // Start is called before the first frame update
    void Start()
    {
        intro = GameObject.FindGameObjectWithTag("MusicIntro").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!intro.isPlaying)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().Play();
            }
        }
    }
}
