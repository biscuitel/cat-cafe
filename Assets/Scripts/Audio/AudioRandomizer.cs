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
        for (int i = 0; i < clips.Count - 1; i++)
        {
            randIndices.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomClip()
    {

        int rand = Random.Range(0, randIndices.Count - 1);
        audioSource.clip = clips[randIndices[rand]];
        audioSource.Play();
        randIndices.RemoveAt(rand);
    }

}
