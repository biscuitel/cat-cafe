using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleOutcomes : Outcomes
{
    private TaskManager taskManager;

    // Start is called before the first frame update
    void Start()
    {
        taskManager = GetComponent<TaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Outcome(int outcomeID)
    {
        // put outcome switch here
        switch (outcomeID)
        {
            case 0:
                // activate task 1 after task 0 is completed
                taskManager.ActivateTask(1);
                break;
            case 1:
                // activate task 2 after task 1 is completed
                taskManager.ActivateTask(2);
                break;
            case 2:
                // activate task group after task 2 is completed
                taskManager.ActivateGroup(0);
                break;
            case 3:
                // all tasks completed
                Debug.Log("All tasks completed!");
                taskManager.SetUIText("All tasks have been completed");
                break;
            default:
                // do outcome 2 stuff here
                break;
        }
    }
}
