using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, CarControls.IPlayerLocomotionMapActions
{
    public CarControls CarControls { get; private set; }//input system
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public float SprintInput { get; private set; }
    public bool AttackPressed { get; private set; }

    //Set up input system
    private void OnEnable()
    {
        CarControls = new CarControls();
        CarControls.Enable();

        CarControls.PlayerLocomotionMap.Enable();
        CarControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        CarControls.PlayerLocomotionMap.Disable();
        //CarControls.PlayerLocomotionMap.RemoveCallbacks(this);    TODO look into this?
    }

    private void LateUpdate()
    {
        JumpPressed = false;//stop jumping
        AttackPressed = false;//stop attacking
    }

    //Detect movement
    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }
        JumpPressed = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInput = context.ReadValue<float>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }
        AttackPressed = true;
    }
}
