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
    private ColorAdjustments colorAdjustments;

    [SerializeField] private float triggerTime = 60f;
    [SerializeField] private float timeElapsed = 0f;
    private bool useTime = false;
    private bool active = false;

    [SerializeField] private float distortTime = 5f;
    [SerializeField] private float maxLensDistortIntensity = -0.5f;
    [SerializeField] private float maxFocalLength = 20f;
    [SerializeField] private float maxChromaAberation = 0.15f;
    [SerializeField] private float maxPaniniProj = 1f;
    [SerializeField] private float maxVignetteIntensity = 0.4f;
    [SerializeField] private float minBloomIntensity = 0.25f;
    [SerializeField] private float maxBloomIntensity = 0.75f;
    [SerializeField] private float postExposure = 1f;
    [SerializeField] private float contrast = 50f;
    [SerializeField] private float hueShift = -55f;
    [SerializeField] private float saturation = 45f;

    // Start is called before the first frame update
    void Start()
    {
        // get postprocess volume and associated override properties
        volumeProfile = GameObject.FindGameObjectWithTag("PostProcessVolume").GetComponent<Volume>().profile;
        volumeProfile.TryGet<LensDistortion>(out ld);
        volumeProfile.TryGet<DepthOfField>(out dof);
        volumeProfile.TryGet<ChromaticAberration>(out chromaAb);
        volumeProfile.TryGet<PaniniProjection>(out paniniProj);
        volumeProfile.TryGet<Vignette>(out vignette);
        volumeProfile.TryGet<Bloom>(out bloom);
        volumeProfile.TryGet<ColorAdjustments>(out colorAdjustments);

        useTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if using time to trigger fx (i.e. after first manual trigger in day 1)
        // trigger fx is elapsed time >= trigger time
        if (useTime && !active)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= triggerTime)
            {
                StartDistort();
                timeElapsed = 0f;
            }
        }
    }

    public void StartDistort()
    {
        if (!active)
        {
            active = true;
            StartCoroutine(Distort());
        }
    }

    public void StartUndistort()
    {
        if (active)
        {
            active = false;
            StartCoroutine(Undistort());
        }
    }

    IEnumerator Distort()
    {
        // activate desired overrides, and lerp to target values for postprocessing fx parameters
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = colorAdjustments.active = true;
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

            colorAdjustments.postExposure.Override(Mathf.Lerp(0f, postExposure, elapsed / distortTime));
            colorAdjustments.contrast.Override(Mathf.Lerp(0f, contrast, elapsed / distortTime));
            colorAdjustments.hueShift.Override(Mathf.Lerp(0f, hueShift, elapsed / distortTime));
            colorAdjustments.saturation.Override(Mathf.Lerp(0f, saturation, elapsed / distortTime));
            yield return null;
        }
    }

    IEnumerator Undistort()
    {
        // lerp to target values for postprocessing fx parameters, then deactivate desired overrides
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

            colorAdjustments.postExposure.Override(Mathf.Lerp(postExposure, 0f, elapsed / distortTime));
            colorAdjustments.contrast.Override(Mathf.Lerp(contrast, 0f, elapsed / distortTime));
            colorAdjustments.hueShift.Override(Mathf.Lerp(hueShift, 0f, elapsed / distortTime));
            colorAdjustments.saturation.Override(Mathf.Lerp(saturation, 0f, elapsed / distortTime));
            yield return null;
        }
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = colorAdjustments.active = false;
    }

    public void TimeTrigger()
    {
        useTime = true;
    }
}
