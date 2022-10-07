using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
    private PlayerMovement pm;

    [SerializeField] private float triggerTime = 45f;
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private float promptTime = 5f;
    private bool useTime = false;
    private bool active = false;
    private bool hasMeds = false;

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

    [SerializeField] private float minSpeed = 1f;
    private float normalSpeed;

    [SerializeField] private Animator pillAnimator;

    public Text promptText;

    private Coroutine distortCoroutine;

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

        ld.intensity.Override(0f);
        dof.focalLength.Override(0f);
        chromaAb.intensity.Override(0f);
        paniniProj.distance.Override(0f);
        vignette.intensity.Override(0f);
        bloom.intensity.Override(0.07f);

        colorAdjustments.postExposure.Override(0f);
        colorAdjustments.contrast.Override(0f);
        colorAdjustments.hueShift.Override(0f);
        colorAdjustments.saturation.Override(0f);

        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        normalSpeed = pm.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // if using time to trigger fx (i.e. after first manual trigger in day 1)
        // trigger fx is elapsed time >= trigger time
        if (!active)
        {
            if (useTime)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= triggerTime)
                {
                    StartDistort();
                    timeElapsed = 0f;
                }
            }
            
        } else {
            if (hasMeds && Input.GetButtonDown("TakeMeds")) {
                DeactivatePrompt();
                StartUndistort();
                pillAnimator.SetTrigger("PillSwallow");
                pillAnimator.SetTrigger("PillReset");

            }
        }
    }

    public void StartDistort()
    {
        if (!active)
        {
            active = true;
            distortCoroutine = StartCoroutine(Distort());
            if (hasMeds)
            {
                StartCoroutine(PromptTimer());
            }
        }
    }

    public void StartUndistort()
    {
        if (active)
        {
            active = false;
            StopCoroutine(distortCoroutine);
            StartCoroutine(Undistort());
            
        }
    }

    public void StartPromptTimer()
    {
        StartCoroutine(PromptTimer());
    }

    IEnumerator PromptTimer()
    {
        float elapsed = 0f;
        while (elapsed < promptTime)
        {
            if (hasMeds && Input.GetButtonDown("TakeMeds"))
            {
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        ActivatePrompt();

    }

    private void ActivatePrompt()
    {
        promptText.text = "Press Q to take an antihistamine.";
    }

    private void DeactivatePrompt()
    {
        promptText.text = "";
    }

    IEnumerator Distort()
    {
        // activate desired overrides, and lerp to target values for postprocessing fx parameters
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = colorAdjustments.active = true;

        float currDistort = ld.intensity.value;
        float currFocal = dof.focalLength.value;
        float currChroma = chromaAb.intensity.value;
        float currPanini = paniniProj.distance.value;
        float currVignette = vignette.intensity.value;
        float currBloom = bloom.intensity.value;

        float currMoveSpeed = pm.moveSpeed;

        float elapsed = 0f;
        while (elapsed <= distortTime)
        {
            elapsed += Time.deltaTime;
            ld.intensity.Override(Mathf.Lerp(currDistort, maxLensDistortIntensity, elapsed/distortTime));
            dof.focalLength.Override(Mathf.Lerp(currFocal, maxFocalLength, elapsed/distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(currChroma, maxChromaAberation, elapsed/distortTime));
            paniniProj.distance.Override(Mathf.Lerp(currPanini, maxPaniniProj, elapsed / distortTime));
            vignette.intensity.Override(Mathf.Lerp(currVignette, maxVignetteIntensity, elapsed / distortTime));
            bloom.intensity.Override(Mathf.Lerp(currBloom, maxBloomIntensity, elapsed / distortTime));

            pm.moveSpeed = Mathf.Lerp(currMoveSpeed, minSpeed, elapsed / distortTime);
            /*colorAdjustments.postExposure.Override(Mathf.Lerp(0f, postExposure, elapsed / distortTime));
            colorAdjustments.contrast.Override(Mathf.Lerp(0f, contrast, elapsed / distortTime));
            colorAdjustments.hueShift.Override(Mathf.Lerp(0f, hueShift, elapsed / distortTime));
            colorAdjustments.saturation.Override(Mathf.Lerp(0f, saturation, elapsed / distortTime));*/
            yield return null;
        }
    }

    IEnumerator Undistort()
    {
        // lerp to target values for postprocessing fx parameters, then deactivate desired overrides
        float elapsed = 0f;

        float currDistort = ld.intensity.value;
        float currFocal = dof.focalLength.value;
        float currChroma = chromaAb.intensity.value;
        float currPanini = paniniProj.distance.value;
        float currVignette = vignette.intensity.value;
        float currBloom = bloom.intensity.value;
        float currMoveSpeed = pm.moveSpeed;

        while (elapsed <= distortTime)
        {
            elapsed += Time.deltaTime;
            ld.intensity.Override(Mathf.Lerp(currDistort, 0f, elapsed / distortTime));
            dof.focalLength.Override(Mathf.Lerp(currFocal, 0f, elapsed / distortTime));
            chromaAb.intensity.Override(Mathf.Lerp(currChroma, 0f, elapsed / distortTime));
            paniniProj.distance.Override(Mathf.Lerp(currPanini, 0f, elapsed / distortTime));
            vignette.intensity.Override(Mathf.Lerp(currVignette, 0f, elapsed / distortTime));
            bloom.intensity.Override(Mathf.Lerp(currBloom, minBloomIntensity, elapsed / distortTime));

            pm.moveSpeed = Mathf.Lerp(currMoveSpeed, normalSpeed, elapsed / distortTime);
            /*colorAdjustments.postExposure.Override(Mathf.Lerp(postExposure, 0f, elapsed / distortTime));
            colorAdjustments.contrast.Override(Mathf.Lerp(contrast, 0f, elapsed / distortTime));
            colorAdjustments.hueShift.Override(Mathf.Lerp(hueShift, 0f, elapsed / distortTime));
            colorAdjustments.saturation.Override(Mathf.Lerp(saturation, 0f, elapsed / distortTime));*/
            yield return null;
        }
        ld.active = dof.active = chromaAb.active = paniniProj.active = vignette.active = colorAdjustments.active = false;
    }

    public void TimeTrigger()
    {
        useTime = true;
    }

    public void SetHasMeds(bool hasMeds)
    {
        this.hasMeds = hasMeds;
        if (active) StartPromptTimer();
    }

    public bool HasMeds()
    {
        return hasMeds;
    }
}
