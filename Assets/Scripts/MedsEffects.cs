using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MedsEffects : MonoBehaviour
{
    [SerializeField] private VolumeProfile volumeProfile;
    private LensDistortion ld;
    private DepthOfField dof;
    private ChromaticAberration chromaAb;
    private PaniniProjection paniniProj;
    private Vignette vignette;
    private Bloom bloom;

    [SerializeField] private float distortTime = 5f;
    [SerializeField] private float maxLensDistortIntensity = -0.5f;
    [SerializeField] private float maxFocalLength = 20f;
    [SerializeField] private float maxChromaAberation = 0.15f;
    [SerializeField] private float maxPaniniProj = 1f;
    [SerializeField] private float maxVignetteIntensity = 0.4f;
    [SerializeField] private float minBloomIntensity = 0.25f;
    [SerializeField] private float maxBloomIntensity = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        volumeProfile = GameObject.FindGameObjectWithTag("PostProcessVolume").GetComponent<Volume>().profile;
        volumeProfile.TryGet<LensDistortion>(out ld);
        volumeProfile.TryGet<DepthOfField>(out dof);
        volumeProfile.TryGet<ChromaticAberration>(out chromaAb);
        volumeProfile.TryGet<PaniniProjection>(out paniniProj);
        volumeProfile.TryGet<Vignette>(out vignette);
        volumeProfile.TryGet<Bloom>(out bloom);
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
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = bloom.active = true;
        float elapsed = 0f;
        while (elapsed < distortTime)
        {
            elapsed += Time.deltaTime;
            ld.intensity.Override(Mathf.Lerp(0f, maxLensDistortIntensity, elapsed/distortTime));
            dof.focalLength.Override(Mathf.Lerp(0f, maxFocalLength, elapsed/distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(0f, maxChromaAberation, elapsed/distortTime));
            paniniProj.distance.Override(Mathf.Lerp(0f, maxPaniniProj, elapsed / distortTime));
            vignette.intensity.Override(Mathf.Lerp(0f, maxVignetteIntensity, elapsed / distortTime));
            bloom.intensity.Override(Mathf.Lerp(minBloomIntensity, maxBloomIntensity, elapsed / distortTime));
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
            dof.focalLength.Override(Mathf.Lerp(maxFocalLength, 0f, elapsed / distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(maxChromaAberation, 0f, elapsed / distortTime));
            paniniProj.distance.Override(Mathf.Lerp(maxPaniniProj, 0f, elapsed / distortTime));
            vignette.intensity.Override(Mathf.Lerp(maxVignetteIntensity, 0f, elapsed / distortTime));
            bloom.intensity.Override(Mathf.Lerp(maxBloomIntensity, minBloomIntensity, elapsed / distortTime));
            yield return null;
        }
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = bloom.active = false;
    }
}
