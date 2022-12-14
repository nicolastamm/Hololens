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
    Dictionary<GameObject, Transform> Elements;
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

        this.widthX = Math.Abs(this.TopRight.position.x - this.BottomRight.position.x);
        this.heightZ = Math.Abs(this.BottomLeft.position.z - this.BottomRight.position.z);

    }
    private void CreateObject(int index, int row, int column, Vector3 offset, Vector3 spacing)
    {
        Debug.Log("width, height: " + widthX + " " + heightZ);
        Transform t = Objects[index];
        Vector3 realScale = t.localScale;
        //realScale.Scale(this.transform.InverseTransformPoint(Vector3.one));
        // Up (y)
        float upPosition = realScale.y / 2;
        // Left/right (x)
        float rightPosition = offset.x + spacing.x * column;// - 2.0f * this.transform.localScale.x ;


        // Forward (z)
        float forwardPosition = (offset.z + spacing.z * row);// -1.5f * this.transform.localScale.z;

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
        this.Elements.Add(go.gameObject, go.transform);

    }
    void Start()
    {
        this.Elements = new Dictionary<GameObject, Transform>();
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
        
        if((obj.transform.position - this.Elements[obj].position).magnitude > obj.transform.localScale.magnitude)
        {
            Debug.Log("spawn new");
            //Spawn new
            GameObject newObject = Instantiate(obj);
            newObject.transform.SetPositionAndRotation(this.Elements[obj].position, this.Elements[obj].rotation);

            obj.transform.SetParent(null, false);
            this.Elements.Remove(obj);
            this.Elements.Add(newObject,  newObject.transform);
            obj?.gameObject.GetComponent<ObjectManipulator>()?.OnManipulationEnded.RemoveListener(this.handleDisplacement);
        }
        else
        {
            Debug.Log("reset position");

            //Reset position
            obj.transform.SetPositionAndRotation(this.Elements[obj].position, this.Elements[obj].rotation);
        }
    }

    private void OnDestroy()
    {
        foreach(Transform g in this.Objects)
        {
            g?.gameObject.GetComponent<ObjectManipulator>()?.OnManipulationEnded.RemoveListener(this.handleDisplacement);
        }
    }
}
