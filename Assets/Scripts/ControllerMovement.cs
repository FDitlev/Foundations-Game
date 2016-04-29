using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public enum ButtonStatus {
Down,
Up
}


public class ControllerMovement : MonoBehaviour {
    public float acceleration = 1;
    public float rotationspeed = 1;
    public float jumpStr = 1;
    Rigidbody sbody;
    public GameObject cam;
    // public GameObject character;
    bool canJump;
    Animator anim;
   public float speed;
    public float vertspeed;
    //private ButtonStatus lastButtonState;
    private ButtonStatus currentButtonState;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
       sbody = GetComponent<Rigidbody>();
        
        Vector3 relative = transform.InverseTransformDirection(sbody.velocity);
       
            


        InputDevice inputDevice = InputManager.ActiveDevice;
       var  player = InputManager.Devices[0];
        if (inputDevice == null)
            return;
        // set button state
      currentButtonState = inputDevice.Action1 ? ButtonStatus.Down : ButtonStatus.Up;
        Vector3 movement = new Vector3(acceleration * Time.deltaTime * player.LeftStickX, 0, acceleration * Time.deltaTime * player.LeftStick.Y);
        Vector3 direction = new Vector3(rotationspeed * Time.deltaTime * player.RightStickY, rotationspeed * Time.deltaTime * player.RightStickX, 0);
        Vector3 jumps = new Vector3(0, jumpStr * Time.deltaTime, 0);

        speed = Mathf.Abs((movement.x + movement.z));
        vertspeed = Mathf.Abs(relative.y);

        anim.SetFloat("SpeedFloat", speed);
        anim.SetFloat("VerticalFloat",vertspeed);
        
        if (sbody.velocity.y <= 0.5f && !canJump)
        {
            canJump = true;
            Debug.Log("CAN JUMP");
            
        }

        
        if (direction != Vector3.zero) {
            sbody.transform.Rotate(0,direction.y,0);
            cam.transform.Rotate(direction.x/3, 0.0f, 0.0f);
            //sbody.rotation = Quaternion.Euler(direction);
           // Debug.Log("Turn");
        }

        if (movement != Vector3.zero) {
            var locVel = transform.InverseTransformDirection(sbody.velocity);
            Vector3 vel = locVel;
            vel.x = (movement.x*5);
            vel.z = (movement.z * 5);
            locVel = vel;
            sbody.velocity = transform.TransformDirection(locVel);
           
            //Debug.Log("Move");
        }
        
        /*if (movement == Vector3.zero && sbody.velocity != Vector3.zero)
        {
            sbody.velocity = Vector3.zero;
        }*/

        if (currentButtonState == ButtonStatus.Down && canJump){
            var locVel = transform.InverseTransformDirection(sbody.velocity);
            Vector3 vel = locVel;
            vel.y = (jumps.y);
          //  canJump = false;
            locVel = vel;
            sbody.velocity = transform.TransformDirection(locVel);
            Debug.Log("Jump");
            canJump = false;
            
        }
        //sbody.velocity = direction;

       /* if (direction == Vector3.zero) {
            sbody.rotation= Quaternion.Euler(direction);
            transform.forward = direction;
        }*/
        //
        // sbody.AddForce(direction);
        //  sbody.transform.position = movement * transform.forward;
       // sbody.rotation = Quaternion.Euler(direction);
       // transform.forward = direction;

    }
}
