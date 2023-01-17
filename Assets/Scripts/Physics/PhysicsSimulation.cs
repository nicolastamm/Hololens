using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulation : MonoBehaviour
{

    [ContextMenu("Play")]
    public void Play()
    {
        foreach (var obj in GameObject.FindObjectsOfType<PhysicsObject>())
        {
            obj.Play();
        }
    }
    [ContextMenu("Pause")]

    public void Pause()
    {
        foreach (var obj in GameObject.FindObjectsOfType<PhysicsObject>())
        {
            obj.Pause();
        }

    }
}
