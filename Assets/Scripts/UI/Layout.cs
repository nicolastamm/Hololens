using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Layout : MonoBehaviour
{
    [SerializeField] Transform[] Objects;
    [SerializeField] int Columns;
    int Rows;
    private void CreateObject(int index, int row, int column, Vector3 offset, Vector3 spacing)
    {
        Transform t = Objects[index];
        Vector3 realScale = t.localScale;
        realScale.Scale(this.transform.InverseTransformPoint(Vector3.one));
        // Up (y)
        float upPosition = realScale.y / 2;
        // Left/right (x)
        float rightPosition = offset.x + spacing.x * column - 5;


        // Forward (z)
        float forwardPosition = 5-(offset.z + spacing.z * row);

        Transform go = Instantiate(Objects[index], this.transform);
        go.localPosition = new Vector3(rightPosition, upPosition, forwardPosition);
        Vector3 temp = this.transform.InverseTransformPoint(Vector3.one);
        temp.Scale(Objects[index].transform.localScale);

        go.localScale = temp;
        go.gameObject.SetActive(true);
    }
    void Start()
    {
        this.Rows = this.Objects.Length / this.Columns;
        //Vector3 maxBounds = this.transform.localPosition + this.transform.localScale;
        Vector3 maxBounds = new Vector3(10.0f, 10.0f, 10.0f);

        Vector3 spacing = maxBounds / this.Columns;
        spacing.z = (maxBounds / (this.Rows + Convert.ToInt32(this.Objects.Length % this.Rows !=0))).z;
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
