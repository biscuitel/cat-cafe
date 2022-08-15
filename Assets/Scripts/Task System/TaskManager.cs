using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskBase> taskList;
    [SerializeField] private List<TaskBase> reserveTaskList;
    public Text taskText;
    public string jsonPath;
    
    // Start is called before the first frame update
    void Start()
    {
        //InitList();
        UpdateUI();
    }

    private void InitList()
    {
        taskList = new List<TaskBase>();
        // populate with tasks according to some imported list of them, or scriptable obj, here
        // or smth idk
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TaskComplete(int taskID)
    {
        // for each task in the group
        foreach (TaskBase task in taskList)
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
                    task.TaskComplete();
                    taskList.Remove(task);
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
                        taskList.Remove(task);
                        break;
                    }
                }
            }
        }
        UpdateUI();
    }

    // handles updating the UI text for tasks
    // should be called any time the tasks list changes or the task UI needs updating
    void UpdateUI()
    {
        // update the task UI here
        StringBuilder sb = new StringBuilder();
        GetAllTaskText(ref taskList, ref sb);
        taskText.text = sb.ToString();
    }

    // iterates over task list, and recursively calls itself for groups of tasks
    // getting task ID + name + desc for tasks, and constructing string for UI text
    void GetAllTaskText(ref List<TaskBase> list, ref StringBuilder sb)
    {
        foreach (TaskBase task in taskList)
        {
            TaskSO taskSO = task as TaskSO;
            if (taskSO != null)
            {
                GetTaskText(taskSO, ref sb);
            }
            else
            {
                TaskGroupSO taskGroupSO = task as TaskGroupSO;
                if (taskGroupSO != null)
                {
                    GetAllTaskText(ref taskGroupSO.taskGroup, ref sb);
                }
            }
        }

    }

    void GetTaskText(TaskSO task, ref StringBuilder sb)
    {
        sb.AppendLine("ID: " + task.taskID + " - " + task.taskName);
        sb.AppendLine(task.taskDesc);
        sb.AppendLine();
    }
}
