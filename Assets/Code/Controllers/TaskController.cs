using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public delegate void Task();

public class TaskController : MonoBehaviour {

    private Queue<Task> TaskQueue = new Queue<Task>();
    private object queueLock = new object();
	
	// Update is called once per frame
	void Update () {
        lock (queueLock) {
            if (TaskQueue.Count > 0) {
                TaskQueue.Dequeue()();
            }
        }
	}

    public void ScheduleTask(Task newTask) {
        lock (queueLock) {
            if (TaskQueue.Count < 10000) {
                TaskQueue.Enqueue(newTask);
            }
        }
    }
}