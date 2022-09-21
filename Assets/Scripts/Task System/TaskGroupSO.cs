using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TaskGroup", order = 1)]
public class TaskGroupSO : TaskBase
{
    public int groupID;
    public int outcomeID;
    private List<TaskBase> taskGroup;
    public List<TaskBase> tasks;
    private TaskManager tm;

    public void Initialize(TaskManager taskManager)
    {
        taskGroup = new List<TaskBase>(tasks);
        tm = taskManager;

    }

    public bool CheckCompletion(int taskID)
    {
        bool returnVal = false;
        // for each task in the group
        foreach (TaskBase task in taskGroup)
        {
            // if the current task is just a task (i.e. not a group)
            // and the ID of the completed task matches the ID provided by the interacted obj
            // complete the task (and any associated actions) and remove it from the list
            // then break from iteration
            TaskSO taskSO = task as TaskSO;
            if (taskSO != null)
            {
                if (taskSO.taskID == taskID)
                {
                    tm.CompletionFromGroup(taskSO.outcomeID);
                    taskGroup.Remove(task);
                    Debug.Log("Removed task with ID: " + taskID);
                    returnVal = true;
                    break;
                }
            }
            else
            {
                // else if current task is a group of tasks
                // call this current function from that group, passing the task ID
                TaskGroupSO taskGroupSO = task as TaskGroupSO;
                if (taskGroupSO != null)
                {
                    returnVal = taskGroupSO.CheckCompletion(taskID);
                    // if all tasks in the group have been completed, execute group finish actions, and remove from this group of tasks
                    // then break from iteration
                    if (taskGroupSO.IsGroupCompleted())
                    {
                        taskGroup.Remove(task);
                    }
                    break;
                }
            }
        }

        return returnVal;
    }

    public bool IsGroupCompleted()
    {
        // if group is empty, all tasks in group have been completed
        if (taskGroup.Count == 0)
        {
            Debug.Log("all tasks in group completed");
            return true;
        } else
        {
            return false;
        }
    }

    public List<TaskBase> GetTaskList()
    {
        return taskGroup;
    }

}
