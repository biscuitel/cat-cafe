using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public movement values
    public float moveSpeed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    private CharacterController controller;

    // -Y distance to check for ground contact
    public float groundDistance = 0.4f;
    public LayerMask[] groundMasks;
    private Transform groundCheck;
    private bool isGrounded;

    // player velocity for purposes of jumping, gravity, grounded checks, etc.
    private Vector3 vel;

    // dialogue manager to disable movement when reading dialogue
    private DialogueManager dm;

    // medseffects script to handle undistorting allergy vfx
    private MedsEffects allergyFX;
    private bool hasMeds = false;

    // Start is called before the first frame update
    void Start()
    {
        // get ground check object from children
        foreach (Transform child in this.transform)
        {
            if (child.name.Equals("GroundCheck"))
            {
                groundCheck = child;
                break;
            }
        }
        
        controller = this.GetComponent<CharacterController>();
        GameObject dmObj = GameObject.FindGameObjectWithTag("DialogueManager");
        if (dmObj != null)
        {
            dm = dmObj.GetComponent<DialogueManager>();
        }
        allergyFX = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<MedsEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        if (dm == null || !dm.IsDialogueActive())
        {
            Inputs();
        }
    }

    void GroundCheck()
    {
        // check that the player is grounded
        foreach (LayerMask mask in groundMasks)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, mask);
        }

        // reset gravity if grounded
        if (isGrounded && vel.y < 0)
        {
            vel.y = -2f;
        }
    }
    void Inputs()
    {
        // get inputs, create movement vector, move player character
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveVec = this.transform.right * x + this.transform.forward * z;
        moveVec.Normalize();

        // jump if player inputs jump and is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            vel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // add gravity to player's y velocity
        vel.y += gravity * Time.deltaTime;
        controller.Move(vel * Time.deltaTime);

        // handle input for player taking meds
        if (hasMeds && Input.GetButtonDown("TakeMeds"))
        {
            allergyFX.StartUndistort();
        }
    }

}
