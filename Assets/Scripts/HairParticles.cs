using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairParticles : MonoBehaviour
{
    private ParticleSystem particles;
    private GameObject particlesParent;
    void Start()
    {
        particlesParent = GameObject.FindGameObjectWithTag("HairParticles");
        particles = particlesParent.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        particlesParent.transform.position = this.transform.position;
        particles.Play();
    }
}
