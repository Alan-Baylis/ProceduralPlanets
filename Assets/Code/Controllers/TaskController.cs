/* Purpose: This class is attatched to the master and is a general controller.
 * 
 * Special Notes: This special controller is used to combine data from other threads into the Unity thread.
 * 
 * Author: Devyn Cyphers; Devcon.
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// Delegates to use.
public delegate void Task();

public class TaskController : MonoBehaviour {

    // Variables to use.
    private Queue<Task> TaskQueue = new Queue<Task>();
    private object queueLock = new object();
	
	// Update. Takes task out of the taskQueue adding them to the Unity thread.
	void Update () {
        lock (queueLock) {
            if (TaskQueue.Count > 0) {
                TaskQueue.Dequeue()();
            }
        }
	}

    // this method takes newTask(Task) and adds it to the taskQueue.
    public void ScheduleTask(Task newTask) {
        lock (queueLock) {
            if (TaskQueue.Count < 10000) {
                TaskQueue.Enqueue(newTask);
            }
        }
    }
}