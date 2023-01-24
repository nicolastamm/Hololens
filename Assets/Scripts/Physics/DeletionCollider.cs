using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeletionCollider : MonoBehaviour
{

    // Find out collisions of PhysicsObject (our created blocks)  with this object, but not with the duplicator
    // ( they are very close so you might collide both of them at the same time )
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PhysicsObject>() != null &&
           !Array.Exists(
               Physics.OverlapBox(collision.transform.position, collision.transform.localScale / 2, Quaternion.identity),
               collider => collider.GetComponent<DuplicateCollider>() != null
           )
       )
        {
            Destroy(collision.gameObject);
        }
    }
}
