using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangedFirst(bool b)
    {
        if (b)
        {

            Debug.Log("Enable FIRST");
        }
        else
        {
            Debug.Log("Disable FIRST");

        }
    }

    public void Changed(bool b)
    {
        if (b)
        {

            Debug.Log("Enable");
        }
        else
        {
            Debug.Log("Disable");

        }
    }
}
