using UnityEngine;
/// <summary>
/// Attach this to all objects that we want to behave as real-life objects
/// </summary>
public class PhysicsObject : MonoBehaviour
{
    /// <summary>
    /// Saves the transform before we hit play, so that when we pause again we can reset their position
    /// </summary>
    Vector3 savedPosition;
    Quaternion savedRotation;
    Vector3 savedScale;
    /// <summary>
    /// Save position before we hit play
    /// </summary>
    void Start()
    {
        this.savedPosition = this.transform.position;
        this.savedRotation = this.transform.rotation;
        this.savedScale = this.transform.localScale;
    }

    /// <summary>
    /// Save position before we hit play
    /// </summary>
    public void Play()
    {
        this.savedPosition = this.transform.position;
        this.savedRotation = this.transform.rotation;
        this.savedScale = this.transform.localScale;

        this.GetComponent<Rigidbody>().isKinematic = false;
    }
    /// <summary>
    /// Restore saved position on pause
    /// </summary>
    public void Pause()
    {
        this.transform.SetPositionAndRotation(this.savedPosition, this.savedRotation);
        this.transform.localScale = this.savedScale;

        this.GetComponent<Rigidbody>().isKinematic = true;

    }
}
