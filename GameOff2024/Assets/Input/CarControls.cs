// GENERATED AUTOMATICALLY FROM 'Assets/Input/CarControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CarControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CarControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CarControls"",
    ""maps"": [
        {
            ""name"": ""PlayerLocomotionMap"",
            ""id"": ""6c658479-8022-4289-b0da-f3a074821240"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ea66a431-c326-44bc-82b9-1410afc178ad"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""81a1c87c-c2c8-4652-95e7-46d3202baa62"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""3402eb56-0aa9-4f4c-a585-228328141824"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""49b2f9ca-88c8-4913-b719-ba6ab081b8c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flash"",
                    ""type"": ""Button"",
                    ""id"": ""c61ef515-ed56-4c97-9f7a-eb81050ad2a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""2a505b44-e665-441a-81f0-0125ff7c5cef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Beep"",
                    ""type"": ""Button"",
                    ""id"": ""d44e6b5d-4e92-4935-919f-e3d7e9c79aba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""bb4f2340-c8e6-4147-98e0-0b451102a148"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9e68bc80-ce83-4329-83ce-22c73fc9dd14"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3c93e6b6-bbbb-41d4-903a-b761823d688b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""df4dc8bc-3580-4644-8f7d-8a80d3db8dd7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""88cf0f8f-ddf8-4f7c-847c-62e42bb91420"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""f12dd84a-aac9-4394-89c8-349c8b2c3c91"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1138c9d1-176b-40cd-ad82-c00c502621e5"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2d1c28b2-a118-40cc-aca2-67d14d452f29"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e75f8d2c-1904-4fed-bf23-c19b4d756921"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3de27f55-301b-4d2c-a190-2597e57693b8"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""0bc29de3-b766-409a-beb8-5f84654473aa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9f591da9-afe5-45a8-a046-754c8485b5a2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1af50e14-ed9b-49ef-a1e6-271620736467"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f48fc371-23d5-4cc4-a82f-3f16c2c49c40"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c81257e7-2a5c-4694-a5a3-28091915d59f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D Pad"",
                    ""id"": ""defc631b-cfdd-4442-85e9-9f2dd6d414be"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9129d38c-9151-46c6-9b3c-725eecdb99a1"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f671a3a5-9762-4396-bab0-347e6e82bbec"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e4e5eefd-7b6e-46b9-b2e4-a0f4475b49d8"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3934c5a0-f2f2-42e6-af4a-2c1e06564025"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2867eb4b-bdf5-49a6-8796-a08355a19af4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba74231d-ff54-4c05-9a38-78685d58a0ee"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=4,y=4)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d7ac9cf-51e4-4c23-a8bc-1950264e63e1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1f99c02-4c1e-4ea7-a905-652b63321acf"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d38849b-a03c-4af0-a74e-497ebc3139f6"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0524449-2e5c-49c8-88e7-241ece135fac"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e912a74-b9e3-44ee-8aeb-2d09f2497b1b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3a28b1e-fdd3-4593-972e-5809e313eac9"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ee92fca-639e-4866-9fd9-a50f9ed082d3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea5c3963-8dee-4c8f-980e-11e567305bd2"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35c5c85b-f809-4bb4-ad6e-78812d5256c6"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Beep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20b26c63-7dfa-4ca0-b906-4d22a2bf77b8"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Beep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerLocomotionMap
        m_PlayerLocomotionMap = asset.FindActionMap("PlayerLocomotionMap", throwIfNotFound: true);
        m_PlayerLocomotionMap_Movement = m_PlayerLocomotionMap.FindAction("Movement", throwIfNotFound: true);
        m_PlayerLocomotionMap_Look = m_PlayerLocomotionMap.FindAction("Look", throwIfNotFound: true);
        m_PlayerLocomotionMap_Jump = m_PlayerLocomotionMap.FindAction("Jump", throwIfNotFound: true);
        m_PlayerLocomotionMap_Sprint = m_PlayerLocomotionMap.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerLocomotionMap_Flash = m_PlayerLocomotionMap.FindAction("Flash", throwIfNotFound: true);
        m_PlayerLocomotionMap_Pause = m_PlayerLocomotionMap.FindAction("Pause", throwIfNotFound: true);
        m_PlayerLocomotionMap_Beep = m_PlayerLocomotionMap.FindAction("Beep", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerLocomotionMap
    private readonly InputActionMap m_PlayerLocomotionMap;
    private IPlayerLocomotionMapActions m_PlayerLocomotionMapActionsCallbackInterface;
    private readonly InputAction m_PlayerLocomotionMap_Movement;
    private readonly InputAction m_PlayerLocomotionMap_Look;
    private readonly InputAction m_PlayerLocomotionMap_Jump;
    private readonly InputAction m_PlayerLocomotionMap_Sprint;
    private readonly InputAction m_PlayerLocomotionMap_Flash;
    private readonly InputAction m_PlayerLocomotionMap_Pause;
    private readonly InputAction m_PlayerLocomotionMap_Beep;
    public struct PlayerLocomotionMapActions
    {
        private @CarControls m_Wrapper;
        public PlayerLocomotionMapActions(@CarControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerLocomotionMap_Movement;
        public InputAction @Look => m_Wrapper.m_PlayerLocomotionMap_Look;
        public InputAction @Jump => m_Wrapper.m_PlayerLocomotionMap_Jump;
        public InputAction @Sprint => m_Wrapper.m_PlayerLocomotionMap_Sprint;
        public InputAction @Flash => m_Wrapper.m_PlayerLocomotionMap_Flash;
        public InputAction @Pause => m_Wrapper.m_PlayerLocomotionMap_Pause;
        public InputAction @Beep => m_Wrapper.m_PlayerLocomotionMap_Beep;
        public InputActionMap Get() { return m_Wrapper.m_PlayerLocomotionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerLocomotionMapActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerLocomotionMapActions instance)
        {
            if (m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnLook;
                @Jump.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnSprint;
                @Flash.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnFlash;
                @Flash.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnFlash;
                @Flash.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnFlash;
                @Pause.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnPause;
                @Beep.started -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnBeep;
                @Beep.performed -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnBeep;
                @Beep.canceled -= m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface.OnBeep;
            }
            m_Wrapper.m_PlayerLocomotionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Flash.started += instance.OnFlash;
                @Flash.performed += instance.OnFlash;
                @Flash.canceled += instance.OnFlash;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Beep.started += instance.OnBeep;
                @Beep.performed += instance.OnBeep;
                @Beep.canceled += instance.OnBeep;
            }
        }
    }
    public PlayerLocomotionMapActions @PlayerLocomotionMap => new PlayerLocomotionMapActions(this);
    public interface IPlayerLocomotionMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnFlash(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnBeep(InputAction.CallbackContext context);
    }
}
