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

    public void Initialize()
    {
        taskGroup = new List<TaskBase>(tasks);
    }

    public bool CheckCompletion(int taskID)
    {
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
                    taskGroup.Remove(task);
                    Debug.Log("Removed task with ID: " + taskID);
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
                    // if all tasks in the group have been completed, execute group finish actions, and remove from this group of tasks
                    // then break from iteration
                    if (taskGroupSO.CheckCompletion(taskID))
                    {
                        taskGroup.Remove(task);
                        break;
                    }
                }
            }
        }

        // if group is empty, all tasks in group have been completed
        // therefore do completion actions, and return true (i.e. group has completed) to parent group or taskmanager
        if (taskGroup.Count == 0)
        {
            Debug.Log("all tasks in group completed");
            return true;
        }

        return false;
    }

    public List<TaskBase> GetTaskList()
    {
        return taskGroup;
    }

}
