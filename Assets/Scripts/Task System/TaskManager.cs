using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskSO> taskList;
    
    // Start is called before the first frame update
    void Start()
    {
        //InitList();
    }

    private void InitList()
    {
        taskList = new List<TaskSO>();
        // populate with tasks according to some imported list of them, or scriptable obj, here
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TaskComplete(int taskID)
    {
        foreach (TaskSO task in taskList)
        {
            if (task.taskID == taskID)
            {
                Debug.Log("Task with ID = " + taskID + " was completed");
                task.TaskComplete();
                taskList.Remove(task);
                break;
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        // update the task UI here
    }
}
