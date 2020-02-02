using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
   [SerializeField]
   private MeshRenderer _callButton;

   [SerializeField]
   private int _requiredCoins = 8;

   private Elevator _elevator;
   private bool _elevatorCalled = false;

   private void Start()
   {
      _elevator = GameObject.Find("Elevator").GetComponent<Elevator>();
      if (_elevator == null)
      {
         Debug.LogError("The elevator object is NULL.");
      }
   }
   private void OnTriggerStay(Collider other)
   {
      // detects trigger collision (this method)
      Player player = other.GetComponent<Player>();

      // if other tag is "Player"
      if (other.tag == "Player")
      {
         // if the "e" key is pressed and required collectables met
         if (Input.GetKeyDown(KeyCode.E) && (player.CoinsCollected() >= _requiredCoins))
         {
            // toggle the panel call button
            if (_elevatorCalled == true)
            {
               _callButton.material.color = Color.red;
            }
            else
            {
               _callButton.material.color = Color.green;
               _elevatorCalled = true;
            }

            // play success audio


            // call and lower the elevator
            _elevator.CallElevator();
         }
         else
         {
            // play fail audio
            //Debug.LogError("Insufficient Coins " + player.CoinsCollected() + ": " + _requiredCoins + " coins are required to call the elevetor.");
         }
      }
   }
}
