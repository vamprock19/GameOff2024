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
    public float runAcceleration = 0.25f;
    public float drag = 0.1f;
    public float runSpeed = 4f;

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
        //calculate move forward in camera direction
        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z).normalized;
        Vector3 moveDirection = cameraRightXZ * playerLocomotionInput.MovementInput.x + cameraForwardXZ * playerLocomotionInput.MovementInput.y;

        Vector3 moveDelta = moveDirection * runAcceleration * Time.deltaTime;
        Vector3 newVel = characterController.velocity + moveDelta;

        //apply drag
        Vector3 curDrag = newVel.normalized * drag * Time.deltaTime;
        newVel = (newVel.magnitude > drag * Time.deltaTime) ? newVel - curDrag : Vector3.zero;
        newVel = Vector3.ClampMagnitude(newVel, runSpeed);

        //move player
        characterController.Move(newVel * Time.deltaTime);
        //when player moving, set animation
        playerAnim.SetBool("isWalking", (playerLocomotionInput.MovementInput.magnitude > 0.1f));

        //jump
        if(playerLocomotionInput.JumpPressed)
        {
            //ToDo Add Jump
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
}
