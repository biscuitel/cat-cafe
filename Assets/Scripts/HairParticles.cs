using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairParticles : MonoBehaviour
{
    private ParticleSystem particlesH;
    private ParticleSystem particlesP;
    private GameObject particlesHair;
    private GameObject particlesPoop;

    [SerializeField] private bool playHair;
    [SerializeField] private bool playPoop;
    void Start()
    {
        particlesHair = GameObject.FindGameObjectWithTag("HairParticles");
        particlesH = particlesHair.GetComponent<ParticleSystem>();
        particlesPoop = GameObject.FindGameObjectWithTag("PoopParticles");
        particlesP = particlesPoop.GetComponent<ParticleSystem>();
    }

   
   
    private void OnDestroy()
    {
        if (particlesHair && playHair)
        {
            particlesHair.transform.position = this.transform.position;
            particlesH.Play();
        }
        if (particlesPoop && playPoop)
        {
            particlesPoop.transform.position = this.transform.position;
            particlesP.Play();
        }
        
    }
}
