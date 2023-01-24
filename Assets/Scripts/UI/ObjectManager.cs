using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEditor;
/// <summary>
/// ObjectManager manages everything for the objects that are inside the block box
/// It provides a CreateObject function that gets called from LayoutManger to instantiate all objects
/// It takes care of color/material assignment
/// It handles the spawning of new objects by drawing them outside of toolbox
/// 
/// Known Issues:
/// Color not working right now
/// 
/// Known Limitations:
/// -
/// 
/// Notes:
/// MUST be started before layout script, configured in script execution order of project
/// </summary>
public class ObjectManager : MonoBehaviour
{
    /// <summary>
    /// Gameobject that we copy the color from
    /// </summary>
    public GameObject ColorObject;
    // Original position and scales of elements
    // NOTE: We store a float instead of vector for scale,
    // because Hololens Interactable elements have buggy behavior with anisotropic scaling
    Dictionary<GameObject, Vector3> Elements;
    Dictionary<GameObject, float> Scales;
    // Reference objects that we want to spawn
    public Transform[] Objects;

    private Layout LayoutManager;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">index in the Objects array</param>
    /// <param name="localPosition">the position to spawn the objects relative to parent container</param>
    public void CreateObject(int index, Vector3 localPosition)
    {
        Transform go = Instantiate(Objects[index], this.transform);

        go.localPosition = localPosition;
        go.gameObject.AddComponent<NearInteractionGrabbable>().ShowTetherWhenManipulating=true;
        go.gameObject.AddComponent<Interactable>();
        go.gameObject.AddComponent<ConstraintManager>();
        go.gameObject.AddComponent<ObjectManipulator>();
        // Layer for PhysicsObjects, when simulation is started/stopped everything in this layer
        // has physics attributes modified
        //go.gameObject.layer = 6; 
        //maybe toglable 
        var rigid = go.gameObject.AddComponent<Rigidbody>();
        rigid.isKinematic = true;
        rigid.useGravity = true;
        go.gameObject.SetActive(true);
        // On manipulation ended, call the function that handles spawning the object from box into world,
        // or placing the object back in the box
        go.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(this.HandleDisplacement);
        // Place the original scale and position of this element
        this.Elements.Add(go.gameObject, go.transform.localPosition);
        this.Scales.Add(go.gameObject, go.transform.localScale.x);

    }
    void Start()
    {
        this.LayoutManager = this.GetComponent<Layout>();

        this.Elements = new Dictionary<GameObject, Vector3>();
        this.Scales = new Dictionary<GameObject, float>();
    }
    /// <summary>
    /// Handles the behavior of drawing an object out of the toolbox
    /// If the drawing distance is small, the object gets placed back into its original position
    /// Otherwise it gets spawned into the world, and a new replica takes its place in the toolbox
    /// </summary>
    /// <param name="data">Data is used to get the object that triggered the event</param>
    public void HandleDisplacement(ManipulationEventData data)
    {
        GameObject obj = data.ManipulationSource;
        // If draw distance greater than object (half)size, put it in the world and spawn the replica in toolbox
        if ((obj.transform.localPosition - this.Elements[obj]).magnitude > obj.transform.localScale.magnitude)
        {

            GameObject newObject = Instantiate(obj, this.transform);
            // Using the original attributes of oldObject
            newObject.transform.localPosition = this.Elements[obj]; 
            newObject.transform.localScale = new Vector3(this.Scales[obj], this.Scales[obj], this.Scales[obj]);
            newObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            newObject.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(this.HandleDisplacement);
            newObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            newObject.gameObject.GetComponent<Rigidbody>().useGravity = true;

            // Remove old object from toolbox and from ObjectManager
            obj.transform.SetParent(null, true);
            this.Elements.Remove(obj);
            this.Scales.Remove(obj);
            obj.gameObject.GetComponent<ObjectManipulator>().OnManipulationEnded.RemoveListener(this.HandleDisplacement);
            // If simulation is already running, the new object should be running as well
            this.GetComponent<PhysicsSimulation>().Append(obj.gameObject.AddComponent<PhysicsObject>());
            // Put the new one in
            this.Elements.Add(newObject, newObject.transform.localPosition);
            this.Scales.Add(newObject, newObject.transform.localScale.x);
        }
        else
        {
            // Reset transform if draw distance too short
            obj.transform.localPosition = this.Elements[obj];
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            obj.transform.localScale = new Vector3(this.Scales[obj], this.Scales[obj], this.Scales[obj]);
        }
    }
    /// <summary>
    /// Updates the color of all objects in the toolbox
    /// </summary>
    public void ColorUpdated()
    {
        Debug.Log("CALLED");
        foreach (GameObject g in this.Elements.Keys)
        {
            g.GetComponent<MeshRenderer>().material.color = this.ColorObject.GetComponent<MeshRenderer>().material.color;

        }
    }
    /// <summary>
    /// Update the material used for all objects in the toolbox
    /// </summary>
    /// <param name="newMaterial"></param>
    public void MaterialUpdated(Material newMaterial)
    {
        foreach (GameObject g in this.Elements.Keys)
        {
            g.GetComponent<MeshRenderer>().material = newMaterial;
            g.GetComponent<MeshRenderer>().material.color = this.ColorObject.GetComponent<MeshRenderer>().material.color;
        }
    }
    // Clean up the listeners we added
    private void OnDestroy()
    {
        foreach (GameObject g in this?.Elements?.Keys)
        {
            g?.gameObject?.GetComponent<ObjectManipulator>()?.OnManipulationEnded?.RemoveListener(this.HandleDisplacement);
        }
    }
}
