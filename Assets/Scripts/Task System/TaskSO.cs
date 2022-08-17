using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Task", order = 1)]
public class TaskSO : TaskBase
{

    public int taskID;
    public string taskName;
    public string taskDesc;
    public string outcomeID;

    public override void TaskComplete()
    {
        // do thing here on completion
    }

}
