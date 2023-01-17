using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    Vector3 savedPosition;
    Quaternion savedRotation;
    Vector3 savedScale;
    // Start is called before the first frame update
    void Start()
    {
        this.savedPosition = this.transform.position;
        this.savedRotation = this.transform.rotation;
        this.savedScale = this.transform.localScale;
    }
    public void Play()
    {
        this.savedPosition = this.transform.position;
        this.savedRotation = this.transform.rotation;
        this.savedScale = this.transform.localScale;

        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Pause()
    {
        this.transform.SetPositionAndRotation(this.savedPosition, this.savedRotation);
        this.transform.localScale = this.savedScale;

        this.GetComponent<Rigidbody>().isKinematic = true;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
