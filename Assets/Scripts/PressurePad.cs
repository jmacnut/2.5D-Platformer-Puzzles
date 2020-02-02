using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
   // disable the box's rigidbody or set kinematic

   private void OnTriggerStay(Collider other)
   {
      // detect moving box
      if (other.tag == "Box")
      {
         // check for distance to center
         float distance = Vector3.Distance(transform.position, other.transform.position);
         Debug.Log("Distance: " + distance);
         // when close to center < 0.05, stop
         if (distance < 0.05f)
         {
            Rigidbody box = other.GetComponent<Rigidbody>();
            if (box != null)
            {
               // stop the box
               box.isKinematic = true;
            }

            MeshRenderer padRenderer = GetComponentInChildren<MeshRenderer>();
            if (padRenderer != null)
            {
               // change color of pressure pad to blue
               padRenderer.material.color = Color.blue;
            }
         }
      }

   }
}
