using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TaskSO : ScriptableObject
{
    [SerializeField] protected string taskName;
    [SerializeField] protected string taskDesc;
    [SerializeField] protected int taskID;

    public abstract void TaskComplete();

}
