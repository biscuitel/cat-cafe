using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTrigger : MonoBehaviour
{

    private TaskManager taskManager;
    [SerializeField] private int taskID;
    [SerializeField] private Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        taskManager = GameObject.FindGameObjectWithTag("TaskManager").GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                CharacterController cc = other.GetComponent<CharacterController>();
                cc.enabled = false;
                other.gameObject.transform.position = destination.position;
                cc.enabled = true;
                Debug.Log("Warped to " + destination.position);
                taskManager.TaskComplete(taskID);
            }
        }
    }}
