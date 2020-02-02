using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
   private CharacterController _controller;

   [SerializeField]
   private float _speed = 5.0f;
   [SerializeField]
   private float _gravity = 1.0f;
   [SerializeField]
   private float _jumpHeight = 25.0f;
   private float _yVelocity;
   private bool _canDoubleJump = false;
   [SerializeField]
   private int _coins;
   private UIManager _uiManager;
   [SerializeField]
   private int _lives = 3;
   private Vector3 _direction;
   private Vector3 _velocity;
   private bool _canWallJump;
   private Vector3 _wallSurfaceNormal;
   [SerializeField]
   private float _pushPower = 2.0f;

   // Start is called before the first frame update
   void Start()
   {
      _controller = GetComponent<CharacterController>();
      _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

      if (_uiManager == null)
      {
         Debug.LogError("The UI Manager is NULL.");
      }

      _uiManager.UpdateLivesDisplay(_lives);
   }

   // Update is called once per frame
   void Update()
   {
      float horizontalInput = Input.GetAxis("Horizontal");

      if (_controller.isGrounded == true)
      {
         _canWallJump = true;
         _direction = new Vector3(horizontalInput, 0, 0);
         _velocity = _direction * _speed;

         if (Input.GetKeyDown(KeyCode.Space))
         {
            _yVelocity = _jumpHeight;
            _canDoubleJump = true;
         }
      }
      else // if player controller is in mid air
      {
         //if (Input.GetKeyDown(KeyCode.Space))
         //{
         //   if (_canWallJump == false)
         //   {
         //      if (_canDoubleJump == true)
         //      {
         //         _yVelocity += _jumpHeight;
         //         _canDoubleJump = false;
         //      }
         //   }
         //   else if (_canWallJump == true)
         //   {
         //      // bounce off wall
         //      _yVelocity = _jumpHeight;
         //      _velocity = _wallSurfaceNormal * _speed;
         //   }
         //}

         // more consistent player behavior than above
         if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == false)
         {
            if (_canDoubleJump == true)
            {
               _yVelocity += _jumpHeight;
               _canDoubleJump = false;
            }
         }

         else if (Input.GetKeyDown(KeyCode.Space) && _canWallJump == true)
         {
            // bounce off wall
            _yVelocity = _jumpHeight;
            _velocity = _wallSurfaceNormal * _speed;
         }

         _yVelocity -= _gravity;
      }

      _velocity.y = _yVelocity;

      _controller.Move(_velocity * Time.deltaTime);
   }

   private void OnControllerColliderHit(ControllerColliderHit hit)
   {
      Rigidbody box;

      // hit the wall in mid air
      if (_controller.isGrounded == false && hit.transform.tag == "Wall")
      {
         Debug.Log("Player is in mid air and hit the wall");
         Debug.DrawRay(hit.point, hit.normal, Color.blue);
         _wallSurfaceNormal = hit.normal;
         _canWallJump = true;
      }

      // if hits block, push block to switch
      if (_controller.isGrounded == true && hit.transform.tag == "Box")
      {
         // push block toward pressure pad
         Debug.Log("Player hit movable box!");


         // confirm rigidbody
         //box = hit.collider.GetComponent<Rigidbody>();
         box = hit.collider.attachedRigidbody;

         //if (box == null  || box.isKinematic)
         //{
         //   return;
         //}

         // don't want to push box below - ??
         //if (hit.moveDirection.y < -0.3f)
         //{
         //   return;
         //}

         if (box != null)
         {
            // calculate move dirction - push sideways, only
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
            
            // apply push
            box.velocity = pushDirection * _pushPower;
         }
      }
   }

   public void AddCoins()
   {
      _coins++;

      _uiManager.UpdateCoinDisplay(_coins);
   }

   public void Damage()
   {
      _lives--;

      _uiManager.UpdateLivesDisplay(_lives);

      if (_lives < 1)
      {
         SceneManager.LoadScene(0);
      }
   }

   public int CoinsCollected()
   {
      return _coins;
   }
}
