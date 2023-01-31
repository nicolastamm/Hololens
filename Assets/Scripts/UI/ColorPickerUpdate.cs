using UnityEngine;

/// <summary>
/// Update color of objects in toolbox using the color from colorpicker
/// </summary>
public class ColorPickerUpdate : MonoBehaviour
{
    public ObjectManager manager;
    Color color;
    void Start()
    {
        color = this.GetComponent<MeshRenderer>().material.color;
    }
    
    void Update()
    {
        var color2 = this.GetComponent<MeshRenderer>().material.color;
        if(color != color2)
        {
            color = color2;
            manager.ColorUpdated();
        }
    }
}
