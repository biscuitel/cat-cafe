using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleTask", order = 1)]
public class ExampleTask : TaskSO
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TaskComplete()
    {
        Debug.Log("completed task with id = " + this.taskID);
    }
}
