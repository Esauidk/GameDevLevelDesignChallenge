using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //Variables
    public float maxSpeed = 6.0F;
    private float effectiveMaxSpeed;
    private float currentJumpSpeed;
    private float currentForwardSpeed;
    private float currentSideSpeed;
    public float jumpSpeed = 8.0F;
    public float jumpBoost = 1.0F;
    public float holdJumpGravityFactor = 0.5f;
    public float acceleration = 12.0F;
    public float aerialAcceleration = 6.0F;
    public float gravity = 20.0F;
    private bool grounded;
    public GameObject cameraObject;
    private Vector3 moveDirection = Vector3.zero;
    private float turner;
    private float looker;
    public float sensitivity;

    private CharacterController controller;

    public float jumpWindow = 0.2f;
    private float jumpBuffer;
    private float coyoteTime;

    void Start()
    {   //Makes it invisable
        Cursor.visible = false;
        //Locks the mouse in place
        Cursor.lockState = CursorLockMode.Locked;
        // In unity press esc to unlock the mouse and unhide it
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        //checks if the controller has hit the ground after a jump, or has left the ground
        CheckForGround();

        // gets the new movement direction based on player input
        CalculateMovementDirection();

        // handles jump logic
        TryJump();

        // moves the camera
        MouseMovement();

        // updates the player controller.
        ApplyMovement();

        Debug.Log(currentForwardSpeed);

    }

	void CheckForGround()
    {
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            if (!grounded)
            {
                // the player has just landed
                grounded = true;
                currentJumpSpeed = 0;

                if(jumpBuffer == 0)
				{
                    effectiveMaxSpeed = maxSpeed;
				}
            }
        }
        else
		{
            if (grounded)
			{
                grounded = false;
                // the player has just left the ground
                coyoteTime = jumpWindow;
			}
		}
    }

    void MouseMovement()
	{
        turner = Input.GetAxis("Mouse X") * sensitivity;
        looker = -Input.GetAxis("Mouse Y") * sensitivity;
        if (turner != 0)
        {
            //Code for action on mouse moving right
            transform.eulerAngles += new Vector3(0, turner, 0);
        }
        if (looker != 0)
        {
            //Code for action on mouse moving right
            cameraObject.transform.eulerAngles += new Vector3(looker, 0, 0);
        }
    }

    void CalculateMovementDirection()
	{
        

        // adjusts the players acceleration depending on if they are in the air or not
        float accelerationValue;
        if (grounded)
		{
            accelerationValue = acceleration;
		}
        else
		{
            accelerationValue = aerialAcceleration;
		}

        // either accelerates or deccelerates the player depending on if they are pressing a movement button or not
		if (Input.GetAxisRaw("Horizontal") == 0)
		{
            // moves current speed toward zero
            if (currentSideSpeed != 0)
			{
                int s = sign(currentSideSpeed);
                currentSideSpeed -= s * accelerationValue * Time.deltaTime;
                if (sign(currentSideSpeed) != s)
                {
                    currentSideSpeed = 0;
                }
            }  
		}
        else
		{
            // allows for reversing direction faster
            if (sign(currentSideSpeed) != Input.GetAxisRaw("Horizontal"))
			{
                accelerationValue *= 3;
			}
            currentSideSpeed = Mathf.Clamp(currentSideSpeed + Input.GetAxisRaw("Horizontal") * accelerationValue * Time.deltaTime, -effectiveMaxSpeed, effectiveMaxSpeed);
        }

        if (Input.GetAxisRaw("Vertical") == 0)
        {
            // moves current speed toward zero
            if (currentForwardSpeed != 0)
            {
                int s = sign(currentForwardSpeed);
                currentForwardSpeed -= s * accelerationValue * Time.deltaTime;
                if (sign(currentForwardSpeed) != s)
                {
                    currentForwardSpeed = 0;
                }
            }
        }
        else
        {
            // allows for reversing direction faster
            if (sign(currentSideSpeed) != Input.GetAxisRaw("Vertical"))
            {
                accelerationValue *= 3;
            }
            currentForwardSpeed = Mathf.Clamp(currentForwardSpeed + Input.GetAxisRaw("Vertical") * accelerationValue * Time.deltaTime, -effectiveMaxSpeed, effectiveMaxSpeed);
        }

        moveDirection = new Vector3(currentSideSpeed, 0, currentForwardSpeed);
        if (moveDirection.magnitude > effectiveMaxSpeed)
		{
            moveDirection = moveDirection.normalized * effectiveMaxSpeed;
		}
        moveDirection = transform.TransformDirection(moveDirection);
    }

    void ApplyMovement()
	{
        //Applying gravity to the controller
        if (!grounded) 
        {
            float g = gravity;
            if (currentJumpSpeed > 0 && Input.GetButton("Jump"))
			{
                g = Mathf.Lerp(gravity * holdJumpGravityFactor, gravity, 1 - currentJumpSpeed / jumpSpeed);
            }

            currentJumpSpeed -= g * Time.deltaTime;
        }
        
        //Making the character move
        controller.Move(new Vector3(moveDirection.x, currentJumpSpeed, moveDirection.z) * Time.deltaTime);
    }

    void TryJump()
	{
        if (Input.GetButtonDown("Jump") || jumpBuffer > 0)
        {
            if (grounded || coyoteTime > 0)
			{
                coyoteTime = 0;
                grounded = false;
                currentJumpSpeed = jumpSpeed;
                moveDirection.y = jumpSpeed;
                if (currentForwardSpeed == effectiveMaxSpeed)
				{
                    effectiveMaxSpeed += jumpBoost;
                }
            }
            else if (jumpBuffer <= 0)
			{
                jumpBuffer = jumpWindow;
			} 
        }

        jumpBuffer = Mathf.Clamp(jumpBuffer - Time.deltaTime, 0, jumpWindow);
        coyoteTime = Mathf.Clamp(coyoteTime - Time.deltaTime, 0, jumpWindow);
    }

    int sign(float f)
	{
        if (Mathf.Approximately(f, 0))
		{
            return 0;
		}
        return (int)(f / Mathf.Abs(f));
	}
}