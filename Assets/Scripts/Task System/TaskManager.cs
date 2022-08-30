using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskBase> taskList;
    [SerializeField] private List<TaskBase> reserveTaskList;
    private Outcomes outcomesScript;
    public Text taskText;
    public string jsonName;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        outcomesScript = GetComponent<Outcomes>();
        UpdateUI();
        /*StringBuilder sb = new StringBuilder();
        ToJson(taskList, ref sb, jsonName);
        sb.Clear();
        ToJson(reserveTaskList, ref sb, jsonName + "Reserve");
        UnityEditor.AssetDatabase.Refresh();*/

        /*FromJson(jsonName);
        FromJson(jsonName + "Reserve");*/
    }

    private void Initialize()
    {
        // initialize tasks groups, if they exist
        foreach (TaskBase task in taskList)
        {
            TaskGroupSO taskGroupSO = task as TaskGroupSO;
            if (taskGroupSO != null)
            {
                taskGroupSO.Initialize();
            }
        }

        foreach (TaskBase task in reserveTaskList)
        {
            TaskGroupSO taskGroupSO = task as TaskGroupSO;
            if (taskGroupSO != null)
            {
                taskGroupSO.Initialize();
            }
        }

    }

    public bool TaskComplete(int taskID)
    {
        bool returnVal = false;
        Debug.Log("taskID = " + taskID + " was triggered");
        // for each task in the group
        foreach (TaskBase task in taskList)
        {
            // if the current task is just a task (i.e. not a group)
            // and the ID of the completed task matches the ID provided by the interacted obj
            // complete the task (and any associated actions) and remove it from the list
            // then break from iteration
            Debug.Log("checking a task in the list");
            TaskSO taskSO = task as TaskSO;
            if (taskSO != null)
            {
                if (taskSO.taskID == taskID)
                {
                    outcomesScript.Outcome(taskSO.outcomeID);
                    taskList.Remove(task);
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
                    Debug.Log("task group is not null");
                    // if all tasks in the group have been completed, execute group finish actions, and remove from this group of tasks
                    // then break from iteration
                    returnVal = taskGroupSO.CheckCompletion(taskID);
                    if (taskGroupSO.IsGroupCompleted())
                    {
                        outcomesScript.Outcome(taskGroupSO.outcomeID);
                        taskList.Remove(task);
                    }
                    break;
                }
            }
        }

        if (taskList.Count > 0)
        {
            UpdateUI();
        }
        return returnVal;
    }

    // handles updating the UI text for tasks
    // should be called any time the tasks list changes or the task UI needs updating
    void UpdateUI()
    {
        // update the task UI here
        StringBuilder sb = new StringBuilder();
        GetAllTaskText(taskList, ref sb);
        taskText.text = sb.ToString();
    }

    // iterates over task list, and recursively calls itself for groups of tasks
    // getting task ID + name + desc for tasks, and constructing string for UI text
    void GetAllTaskText(List<TaskBase> list, ref StringBuilder sb)
    {
        foreach (TaskBase task in list)
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
                    GetAllTaskText(taskGroupSO.GetTaskList(), ref sb);
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

    void ToJson(List<TaskBase> taskList, ref StringBuilder sb, string fileName)
    {
        foreach (TaskBase task in taskList)
        {
            TaskSO taskSO = task as TaskSO;
            if (taskSO != null)
            {
                Debug.Log(taskSO.taskID);
                sb.AppendLine(JsonUtility.ToJson(taskSO, true));
            }
            else
            {
                TaskGroupSO taskGroupSO = task as TaskGroupSO;
                if (taskGroupSO != null)
                {
                    sb.AppendLine(JsonUtility.ToJson(taskGroupSO, true));
                    sb.AppendLine("[");
                    ToJson(taskGroupSO.GetTaskList(), ref sb, jsonName);
                    sb.AppendLine("]");
                }
            }
        }

        using (FileStream fs = new FileStream(Path.Combine(Application.dataPath, "Resources/SavedTasks/" + fileName + ".json"), FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(sb.ToString());
                writer.Close();
            }
            fs.Close();
        }

    }

    void FromJson(string fileName)
    {
        Debug.Log(Path.Combine(Application.dataPath, "Resources/SavedTasks/" + fileName + ".json"));
        TextAsset textAsset = Resources.Load<TextAsset>("SavedTasks/" + fileName);
        Debug.Log(textAsset.ToString());
    }

    // takes a task ID, checks the reserve task list for the ID, and activates the task with matching ID if present
    public void ActivateTask(int taskID)
    {
        foreach (TaskBase task in reserveTaskList)
        {
            TaskSO taskSO = task as TaskSO;
            if (taskSO != null)
            {
                if (taskSO.taskID == taskID)
                {
                    taskList.Add(task);
                    reserveTaskList.Remove(task);
                    break;
                }
            }
        }
    }

    // takes a group ID, checks the reserve task list for the ID, and activates the group with matching ID if present
    public void ActivateGroup(int groupID)
    {
        foreach (TaskBase task in reserveTaskList)
        {
            TaskGroupSO taskGroupSO = task as TaskGroupSO;
            if (taskGroupSO != null)
            {
                if (taskGroupSO.groupID == groupID)
                {
                    taskList.Add(task);
                    reserveTaskList.Remove(task);
                    break;
                }
            }
        }
    }

    public void TriggerOutcome(int outcomeID)
    {
        outcomesScript.Outcome(outcomeID);
    }

    public void SetUIText(string text)
    {
        Debug.Log(text);
        taskText.text = text;
    }

}
