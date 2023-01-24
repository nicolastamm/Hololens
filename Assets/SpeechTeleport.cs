using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTeleport : MonoBehaviour
{
    public void Teleport()
    {
        this.transform.position = new Vector3(-0.1f, -0.1f, 0.7f);
    }
}
