using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MedsEffects : MonoBehaviour
{
    private VolumeProfile volumeProfile;
    private LensDistortion ld;
    private DepthOfField dof;
    private ChromaticAberration chromaAb;
    private PaniniProjection paniniProj;

    [SerializeField] private float distortTime = 5f;
    [SerializeField] private float maxLensDistortIntensity = -0.25f;
    [SerializeField] private float minFocusDistance = 0f;
    [SerializeField] private float maxFocalLength = 30f;
    [SerializeField] private float maxChromaAberation = 0.15f;
    [SerializeField] private float maxPaniniProj = 1f;

    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = GameObject.FindGameObjectWithTag("PostProcessVolume").GetComponent<Volume>().profile;
        volumeProfile.TryGet<LensDistortion>(out ld);
        volumeProfile.TryGet<DepthOfField>(out dof);
        volumeProfile.TryGet<ChromaticAberration>(out chromaAb);
        volumeProfile.TryGet<PaniniProjection>(out paniniProj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDistort()
    {
        StartCoroutine(Distort());
    }

    public void StartUndistort()
    {
        StartCoroutine(Undistort());
    }

    IEnumerator Distort()
    {
        ld.active = dof.active = chromaAb.active = paniniProj.active = true;
        float elapsed = 0f;
        while (elapsed < distortTime)
        {
            elapsed += Time.deltaTime;
            ld.intensity.Override(Mathf.Lerp(0f, maxLensDistortIntensity, elapsed/distortTime));
            dof.focusDistance.Override(Mathf.Lerp(3f, minFocusDistance, elapsed / distortTime));
            dof.focalLength.Override(Mathf.Lerp(0f, maxFocalLength, elapsed/distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(0f, maxChromaAberation, elapsed/distortTime));
            paniniProj.distance.Override(Mathf.Lerp(0f, maxPaniniProj, elapsed / distortTime));
            yield return null;
        }
    }

    IEnumerator Undistort()
    {
        float elapsed = 0f;
        while (elapsed < distortTime)
        {
            elapsed += Time.deltaTime;
            ld.intensity.Override(Mathf.Lerp(maxLensDistortIntensity, 0f, elapsed / distortTime));
            dof.focusDistance.Override(Mathf.Lerp(minFocusDistance, 3f, elapsed / distortTime));
            dof.focalLength.Override(Mathf.Lerp(maxFocalLength, 0f, elapsed / distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(maxChromaAberation, 0f, elapsed / distortTime));
            paniniProj.distance.Override(Mathf.Lerp(maxPaniniProj, 0f, elapsed / distortTime));
            yield return null;
        }
        ld.active = dof.active = chromaAb.active = paniniProj.active = false;
    }
}
