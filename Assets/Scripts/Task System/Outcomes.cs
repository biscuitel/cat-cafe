using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Outcomes : MonoBehaviour
{
    public abstract void Outcome(int outcomeID);

    protected IEnumerator PitchSlide(AudioSource audio, float targetPitch, float slideTime)
    {
        float elapsed = 0f;
        while (elapsed < slideTime)
        {
            elapsed += Time.deltaTime;
            audio.pitch = Mathf.SmoothStep(1f, targetPitch, elapsed / slideTime);
            yield return null;
        }
    }
}
