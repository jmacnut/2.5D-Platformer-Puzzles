using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
   [SerializeField]
   private Transform _upperLevel;

   [SerializeField]
   private Transform _lowerLevel;

   private bool _goingDown = false;

   [SerializeField]
   private float _speed = 2.5f;

   private float _step;

   private void Start()
   {
      _step = _speed * Time.deltaTime;
   }

   public void CallElevator()
   {
      // know current position of the elevator

      // toggle:
      _goingDown = !_goingDown;
   }

   // for smooth stepping
   private void FixedUpdate()
   {
      // if going down == true:
      if (_goingDown == true)
      {
         // take current position = move toward target position
         transform.position = Vector3.MoveTowards(transform.position, _lowerLevel.position, _step);
      }
      else if (_goingDown == false)
      {
         // take current position move toward target
         transform.position = Vector3.MoveTowards(transform.position, _upperLevel.position, _step);
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      // anti-jittering remedy
      if (other.tag == "Player")
      {
         other.transform.SetParent(this.transform);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      // anti jittering remedy
      if (other.tag == "Player")
      {
         other.transform.SetParent(null);
      }
   }
}
