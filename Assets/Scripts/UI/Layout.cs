using UnityEngine;
using System;
/// <summary>
/// Layout class is used to figure out where our objects should be placed
/// Given bounds on a 3d container, it tries to place the objects in a grid layout on top of the container
/// User needs to input the number of rows for layout in the editor
/// 
/// Known Issues:
/// -
/// 
/// Known Limitations:
/// - You should have more columns than elements, or it will throw exception
/// - For some column values the layout offsets don't work, but should be okay for reasonable column/row sizes,
///   it would take too much time to make the script robust to these limitations
/// Notes:
/// MUST be after the objectmanager script, configured in script execution order of project
/// </summary>
public class Layout : MonoBehaviour
{
    /// Width and height of the simluated grid
    float widthX;
    float heightZ;

    // Helper objects to figure out the width/height
    // Placed at bounds of the objcet in the scene
    Transform TopLeft;
    Transform TopRight;
    Transform BottomLeft;
    Transform BottomRight;

    // User must specify how many columns for layout, rows are calculated
    [SerializeField] int Columns;
    // ObjManager keeps/spawns the actual objects
    private ObjectManager ObjManager;
    int Rows;
    /// <summary>
    /// Figures out the width(x-dimension) and height(z-dimension) of our container
    /// </summary>
    private void CalculateBounds()
    {
        this.TopLeft = this.transform.Find("TopLeft");
        this.TopRight = this.transform.Find("TopRight");
        this.BottomLeft = this.transform.Find("BottomLeft");
        this.BottomRight = this.transform.Find("BottomRight");
        // Only needed to figureout bounds, disabled 
        this.TopLeft.gameObject.SetActive(false);
        this.TopRight.gameObject.SetActive(false);
        this.BottomLeft.gameObject.SetActive(false);
        this.BottomRight.gameObject.SetActive(false);

        this.widthX = Math.Abs(Math.Abs(this.TopRight.localPosition.x) + Math.Abs(this.BottomRight.localPosition.x));
        this.heightZ = Math.Abs(Math.Abs(this.BottomLeft.localPosition.z) + Math.Abs(this.BottomRight.localPosition.z));
    }
    private void CaculateObjectPosition(int index, int row, int column, Vector3 offset, Vector3 spacing)
    {
        // Objects should be placed on top of the box
        // Constants were a bit trial and error and might change if you replace the box object
        Transform t = ObjManager.Objects[index];
        Vector3 realScale = t.localScale;

        // Up (y)
        float upPosition = realScale.y;
        // Left/right (x)
        float rightPosition = offset.x + spacing.x * column - 2.35f;

        // Forward (z)
        float forwardPosition = offset.z + spacing.z * row - 1.5f;

        // After figuring out the position, call for objmanager to do the spawning
        ObjManager.CreateObject(index, new Vector3(rightPosition, upPosition, forwardPosition));
    }
    void Start()
    {
        this.ObjManager = this.gameObject.GetComponent<ObjectManager>();
        this.Rows = this.ObjManager.Objects.Length / this.Columns;
        this.CalculateBounds();

        float spacingX = this.widthX / this.Columns;
        float spacingZ = this.heightZ / (this.Rows + Convert.ToInt32(this.ObjManager.Objects.Length % this.Rows !=0));
        Vector3 spacing = new Vector3(spacingX , 0, spacingZ);
        Vector3 offset = spacing / 2;
        // Calcualtes where the objects should be spawned, and passes information to objmanager to do the spawning
        #region Calculate+Spawn
        int index = 0;
        int row = 0; int column = 0;

        
        for (row = 0; row < this.Rows; row++)
        {
            for (column = 0; column < this.Columns; column++)
            {
                index = this.Columns * row + column;
                
                this.CaculateObjectPosition(index, row, column, offset, spacing);
            }
        }
        // Leftover elements as result of truncation
        for (column = 0; column < this.Columns; column++)
        {
            index = this.Columns * row + column;
            if(index >= this.ObjManager.Objects.Length)
            {
                break;
            }
        
            this.CaculateObjectPosition(index, row, column, offset, spacing);
        }
        #endregion
        // Rotate the box 45 degree towards user so its easier to see
        this.transform.Rotate(new Vector3(1, 0, 0), -45);
    }


}
