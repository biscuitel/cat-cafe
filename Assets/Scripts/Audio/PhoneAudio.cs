using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAudio : MonoBehaviour
{
    AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play_sound()
    {
        Audio.Play();
    }
}
