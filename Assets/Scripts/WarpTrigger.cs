using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTrigger : MonoBehaviour
{

    private TaskManager taskManager;
    [SerializeField] private int taskID;
    private Transform destination;

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
                other.transform.position = new Vector3(other.transform.position.x, 67f, other.transform.position.z);
                cc.enabled = true;
                Debug.Log("WARPED????!1");
                //taskManager.TaskComplete(taskID);
            }
        }
        
    }}
