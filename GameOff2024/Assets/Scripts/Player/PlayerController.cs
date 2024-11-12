using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator playerAnim;


    [Header("Movement")]
    public float runAcceleration;
    public float drag;
    public float runSpeed;
    [SerializeField] float sprintMult;

    private float gravity = -9.81f;
    [SerializeField] float gravMult = 3;
    private float verticalVelocity = 0;
    [SerializeField] float jumpStrength;
    private float coyoteTime;
    private float coyoteThreshold = 1.0f;

    private Vector3 movementToApply;

    [Header("Abilities")]
    [SerializeField] float flashCooldown = 1;
    private float flashCooldownTimer;


    [Header("Camera")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private PlayerLocomotionInput playerLocomotionInput;
    

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        movementToApply = new Vector3(0, 0, 0);
        HandleGravity();
        HandleJump();
        HandleCameraAndMovement();
        HandleFlash();

        //set animation parameters
        playerAnim.SetBool("isWalking", (playerLocomotionInput.MovementInput.magnitude > 0.1f));
        playerAnim.SetBool("isSprinting", (playerLocomotionInput.SprintInput > 0.1f));
        if(verticalVelocity < -10)//if falling
        {
            if(!playerAnim.GetBool("isFalling"))
            {
                playerAnim.SetBool("isFalling", true);
                playerAnim.SetBool("isJumping", false);
            }
        }
    }

    //calculate camera after movement
    private void LateUpdate()
    {
        //when player moving
        if(playerLocomotionInput.MovementInput.magnitude > 0.1f)
        {
            Vector3 inpDirection = new Vector3(playerLocomotionInput.MovementInput.x, 0, playerLocomotionInput.MovementInput.y).normalized;

            //set the rotation to facing direction
            float targAng = Mathf.Atan2(inpDirection.x, inpDirection.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targAng, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }
    }

    private void HandleGravity()//calculate downward force
    {
        if(characterController.isGrounded && verticalVelocity < 0)//if on the floor and moving downwards
        {
            verticalVelocity = -1;
            playerAnim.SetBool("isFalling", false);
        }
        else
        {
            verticalVelocity += gravity * gravMult * Time.deltaTime;
        }
    }

    private void HandleCameraAndMovement()
    {
        //calculate move forward in camera direction
        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
        Vector3 moveDirection = cameraRightXZ * playerLocomotionInput.MovementInput.x + cameraForwardXZ * playerLocomotionInput.MovementInput.y;

        Vector3 moveDelta = moveDirection * runAcceleration * Time.deltaTime * ((playerLocomotionInput.SprintInput > 0.1f) ? sprintMult : 1);//if sprinting, multiply acceleration
        Vector3 newVel = characterController.velocity + moveDelta;
        newVel.y = 0;//no vertical speed limit

        //apply drag
        Vector3 curDrag = newVel.normalized * drag * Time.deltaTime;
        newVel = (newVel.magnitude > drag * Time.deltaTime) ? newVel - curDrag : Vector3.zero;
        newVel = (playerLocomotionInput.SprintInput > 0.1f) ? Vector3.ClampMagnitude(newVel, runSpeed * sprintMult) : Vector3.ClampMagnitude(newVel, runSpeed);//if sprinting, multiply speed when clamping

        //apply movement
        movementToApply += newVel;
        movementToApply.y += verticalVelocity;//and gravity and jump
        //move player
        characterController.Move(movementToApply * Time.deltaTime);
    }

    private void HandleJump()
    {
        //coyote time check
        if(characterController.isGrounded)
        {
            coyoteTime = 0;
        }
        else
        {
            coyoteTime += Time.deltaTime;
        }
        //jump
        if(playerLocomotionInput.JumpPressed)
        {
            if(coyoteTime <= coyoteThreshold)//if on ground or in coyote time
            {
                coyoteTime = 10;
                verticalVelocity = jumpStrength;
                playerAnim.SetBool("isFalling", false);
                playerAnim.SetBool("isJumping", true);
            }
        }
    }

    private void HandleFlash()
    {
        flashCooldownTimer -= Time.deltaTime;
        if(flashCooldownTimer <= 0)
        {
            if(playerLocomotionInput.FlashPressed)
            {
                playerAnim.SetTrigger("Flash");
                flashCooldownTimer = flashCooldown;
                
            }
        }
    }

}
