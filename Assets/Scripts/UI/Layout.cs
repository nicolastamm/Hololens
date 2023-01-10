using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.Experimental.AI;

public class Layout : MonoBehaviour
{
    float widthX;
    float heightZ;
    Transform TopLeft;
    Transform TopRight;
    Transform BottomLeft;
    Transform BottomRight;
    Dictionary<GameObject, Vector3> Elements;
    Dictionary<GameObject, float> Scales;

    [SerializeField] Transform[] Objects;
    [SerializeField] int Columns;
    int Rows;
    private void GetBounds()
    {
        this.TopLeft     = this.transform.Find("TopLeft");
        this.TopRight    = this.transform.Find("TopRight");
        this.BottomLeft  = this.transform.Find("BottomLeft");
        this.BottomRight = this.transform.Find("BottomRight");

        this.TopLeft.gameObject.SetActive(false);
        this.TopRight.gameObject.SetActive(false);
        this.BottomLeft.gameObject.SetActive(false);
        this.BottomRight.gameObject.SetActive(false);

        this.widthX = Math.Abs(Math.Abs(this.TopRight.localPosition.x) + Math.Abs(this.BottomRight.localPosition.x));
        this.heightZ = Math.Abs(Math.Abs(this.BottomLeft.localPosition.z) + Math.Abs(this.BottomRight.localPosition.z));
        Debug.Log("TopRight" + TopRight.localPosition);
        Debug.Log("BottomRight" + BottomRight.localPosition);
        Debug.Log("BottomLeft" + BottomLeft.localPosition);

        Debug.Log("width, height: " + widthX + " " + heightZ);
    }
    private void CreateObject(int index, int row, int column, Vector3 offset, Vector3 spacing)
    {
        Transform t = Objects[index];
        Vector3 realScale = t.localScale;
        //realScale.Scale(this.transform.InverseTransformPoint(Vector3.one));
        // Up (y)
        float upPosition = realScale.y;
        // Left/right (x)
        float rightPosition = offset.x + spacing.x * column - 2.35f;// - 2.0f * this.transform.localScale.x ;


        // Forward (z)
        float forwardPosition = (offset.z + spacing.z * row) - 1.5f;// -1.5f * this.transform.localScale.z;

        Transform go = Instantiate(Objects[index], this.transform);
        go.localPosition = new Vector3(rightPosition, upPosition, forwardPosition);

        //Vector3 temp = this.transform.InverseTransformPoint(Vector3.one);
        //temp.Scale(Objects[index].transform.localScale);

        //go.localScale = temp;
        //go.Rotate(45f, 0f, 0f);

        go.gameObject.AddComponent<NearInteractionGrabbable>();
        go.gameObject.AddComponent<Interactable>();
        go.gameObject.AddComponent<ConstraintManager>();
        go.gameObject.AddComponent<ObjectManipulator>();
        go.gameObject.SetActive(true);

        go.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(this.handleDisplacement);
        this.Elements.Add(go.gameObject, go.transform.localPosition);
        this.Scales.Add(go.gameObject, go.transform.localScale.x);

    }
    void Start()
    {
        this.Elements = new Dictionary<GameObject, Vector3>();
        this.Scales= new Dictionary<GameObject, float>();

        this.Rows = this.Objects.Length / this.Columns;
        //Vector3 maxBounds = this.transform.localPosition + this.transform.localScale;
        this.GetBounds();

        float spacingX = this.widthX / this.Columns;
        float spacingZ = this.heightZ / (this.Rows + Convert.ToInt32(this.Objects.Length % this.Rows !=0));
        Vector3 spacing = new Vector3(spacingX , 0, spacingZ);
        Vector3 offset = spacing / 2;

        int index = 0;
        int row = 0; int column = 0;
        for (row = 0; row < this.Rows; row++)
        {
            for (column = 0; column < this.Columns; column++)
            {
                index = this.Columns * row + column;
                
                this.CreateObject(index, row, column, offset, spacing);
            }
        }
        // Extra row
        for (column = 0; column < this.Columns; column++)
        {
            index = this.Columns * row + column;
            if(index >= this.Objects.Length)
            {
                break;
            }
        
            this.CreateObject(index, row, column, offset, spacing);
        }

        this.transform.Rotate(new Vector3(1, 0, 0), -45);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void handleDisplacement(ManipulationEventData data)
    {
        GameObject obj = data.ManipulationSource;
        
        if((obj.transform.localPosition - this.Elements[obj]).magnitude > obj.transform.localScale.magnitude)
        {
            Debug.Log("spawn new");
            //Spawn new
            GameObject newObject = Instantiate(obj, this.transform);
            newObject.transform.localPosition= this.Elements[obj];
            newObject.transform.localScale = new Vector3(this.Scales[obj], this.Scales[obj], this.Scales[obj]);
            newObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            newObject.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(this.handleDisplacement);

            obj.transform.SetParent(null, true);
            this.Elements.Remove(obj);
            this.Elements.Add(newObject, newObject.transform.localPosition);


            this.Scales.Remove(obj);
            this.Scales.Add(newObject, newObject.transform.localScale.x);

            obj?.gameObject?.GetComponent<ObjectManipulator>()?.OnManipulationEnded?.RemoveListener(this.handleDisplacement);

        }
        else
        {
            Debug.Log("reset position");
            Debug.Log("ORIGINAL POS+ " + this.Elements[obj]);
            Debug.Log("now POS+ " + obj.transform.localPosition);


            //Reset position
            obj.transform.localPosition = this.Elements[obj];//.SetPositionAndRotation(this.Elements[obj].position, this.Elements[obj].rotation);
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            obj.transform.localScale = new Vector3(this.Scales[obj], this.Scales[obj], this.Scales[obj]);

        }
    }
    public void ColorUpdated()
    {
        foreach (GameObject g in this.Elements.Keys)
        {
            g.GetComponent<MeshRenderer>().material.color = GameObject.Find("3DButton").GetComponent<MeshRenderer>().material.color;

        }
    }
    public void MaterialUpdated(Material newMaterial)
    {
        foreach (GameObject g in this.Elements.Keys)
        {
            g.GetComponent<MeshRenderer>().material = newMaterial;
            g.GetComponent<MeshRenderer>().material.color = GameObject.Find("3DButton").GetComponent<MeshRenderer>().material.color;

        }
    }
    private void OnDestroy()
    {
        foreach(GameObject g in this.Elements.Keys)
        {
            g?.gameObject?.GetComponent<ObjectManipulator>()?.OnManipulationEnded?.RemoveListener(this.handleDisplacement);
        }
    }
}
