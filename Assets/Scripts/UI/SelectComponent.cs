using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectComponent : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject SelectedObject;
    public void SelectObject(GameObject obj) { 
        this.SelectedObject = obj;
    }
    public void DeselectObject() { }

    public void Debugger()
    {
        Debug.Log("Pressed");

    }
}
