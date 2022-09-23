using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    public float minRandom;
    public float maxRandom;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Invoke("StartAnimation", Random.Range(minRandom, maxRandom)); 
        animator.SetFloat("Offset", Random.Range(minRandom, maxRandom));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartAnimation()
    {
        animator.SetBool("Start", true);

    }
}
