using UnityEngine;
using System;
/// <summary>
/// Used for duplicating objects
/// </summary>
public class DuplicateCollider : MonoBehaviour
{
    /// <summary>
    /// Only allow for collision once every 'timer' seconds
    /// </summary>
    float timer = 0f;

    public void Update()
    {
        timer += Time.deltaTime;
    }

    // Find out collisions of PhysicsObject (our created blocks) with this object, but not with the deleter
    // ( they are very close so you might collide both of them at the same time )
    public void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other.GetComponent<PhysicsObject>() != null &&
            !Array.Exists(
                Physics.OverlapBox(other.transform.position, other.transform.localScale, Quaternion.identity),
                collider => collider.GetComponent<DeletionCollider>() != null
            )
        )
        {
            // Spawn only once every second
            if (timer > 1f)
            {
                // Body isntantiates non-kinematic after duplication
                Instantiate(other, other.transform.position - new Vector3(0.1f, 0.05f, 0.0f), Quaternion.identity)
                    .GetComponent<Rigidbody>().isKinematic = true;
                timer = 0f;
            }
        }
    }
}
