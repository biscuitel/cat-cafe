using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float revIn = 0.5f;
    [SerializeField] private float revLength = 2.0f;
    [SerializeField] private float pitchChange = 0.4f;
    [SerializeField] private float minVol = 0.75f;
    [SerializeField] private float volChange = 0.25f;
    private bool justEnabled;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = minVol;
    }

    private void OnEnable()
    {
        justEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (justEnabled)
        {
            audioSource.Play();
            justEnabled = false;
        }
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("clicked interact on vacuum cleaner");
            StartCoroutine(Rev());
        }
    }

    IEnumerator Rev()
    {
        float vol = audioSource.volume;
        float pitch = audioSource.pitch;
        float elapsed = 0f;
        while (elapsed < revIn)
        {
            elapsed += Time.deltaTime;
            audioSource.pitch = Mathf.SmoothStep(pitch, 1.0f + pitchChange, elapsed / revIn);
            audioSource.volume = Mathf.SmoothStep(vol, minVol + volChange, elapsed / revIn);
            Debug.Log(audioSource.pitch);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < revLength)
        {
            elapsed += Time.deltaTime;
            audioSource.pitch = Mathf.SmoothStep(1.0f + pitchChange, 1.0f, elapsed / revLength);
            audioSource.volume = Mathf.SmoothStep(minVol + volChange, minVol, elapsed / revIn);
            Debug.Log(audioSource.pitch);
            yield return null;
        }
    }
}
