using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TaskSO : ScriptableObject
{
    public string taskName;
    public string taskDesc;
    public int taskID;

    public abstract void TaskComplete();

}
