using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsSimulation : MonoBehaviour
{
    // ToExcute is used to enable running gameobjects when they are spawned while simulation is running
    List<PhysicsObject> ToExecute;
    public bool IsRunning;
    [ContextMenu("Play")]
    public void Play()
    {
        foreach (var obj in GameObject.FindObjectsOfType<PhysicsObject>())
        {
            obj.Play();
        }
        IsRunning = true;

    }
    [ContextMenu("Pause")]

    public void Pause()
    {
        foreach (var obj in GameObject.FindObjectsOfType<PhysicsObject>())
        {
            obj.Pause();
        }
        IsRunning = false;

    }

    private void Start()
    {
        IsRunning = false;
        ToExecute = new List<PhysicsObject>();
    }
    public void Append(PhysicsObject p) {
        ToExecute.Add(p);
    }

    private void Update()
    {
        if (IsRunning)
        {
            foreach (var obj in ToExecute)
            {
                obj.Play();
            }
        }
        ToExecute.Clear();

    }
}
