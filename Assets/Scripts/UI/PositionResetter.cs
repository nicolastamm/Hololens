using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Resets the transform of object
/// Currently used because:
///     If the toolbox has been translated, when you teleport it using your hand, the translation is preserved
///     This would mean it is not spawned on top of our palm, but at an offset
///     So always call the TransformReset function to get the expected functionality
/// NOTE:
///     Should be attached to the object that is "Host Transform" target (ObjectManipulator component)
/// </summary>
public class PositionResetter : MonoBehaviour
{
    public void TransformReset()
    {
        this.transform.localPosition = Vector3.zero;
    }
}
