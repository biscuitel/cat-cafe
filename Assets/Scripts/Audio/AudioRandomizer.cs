using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{

    [SerializeField] private List<AudioClip> clips;
    private AudioSource audioSource;
    private List<int> randIndices;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomClip()
    {
      
        int rand = Random.Range(0, clips.Count);
        float randPitch = Random.Range(0.9f, 1.1f);
        audioSource.clip = clips[rand];
        audioSource.pitch = randPitch;
        audioSource.Play();
    }

}
