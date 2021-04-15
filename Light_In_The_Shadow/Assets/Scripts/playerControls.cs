// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/player scripts/playerController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""playerController"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""04ae807c-4be3-46c5-8b3c-6d2b7eae9efa"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7c80dd9a-c652-4e7d-9313-0971d51dd959"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e98c87f2-1420-4cb6-9fc5-ace331cb08e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenInventory"",
                    ""type"": ""Button"",
                    ""id"": ""dfe4e83d-710f-42f0-be3a-68fc9a457bd7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PickUp"",
                    ""type"": ""Button"",
                    ""id"": ""a8cdd144-0084-48ee-bbb4-826fc50ce781"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""HighlightObject"",
                    ""type"": ""Button"",
                    ""id"": ""0a744d71-e088-46cb-98af-da45e13fb313"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""rotateObject"",
                    ""type"": ""Value"",
                    ""id"": ""e1edb997-53f8-4d1a-b43c-e9d3c68445db"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PlayPause"",
                    ""type"": ""Button"",
                    ""id"": ""d5231fd2-f0fd-4430-aec5-6bac5b2c5a6b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Torch"",
                    ""type"": ""Button"",
                    ""id"": ""26c5cc1a-e637-4a91-a17f-1daa96892cda"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.01)""
                },
                {
                    ""name"": ""BreakingIce"",
                    ""type"": ""Button"",
                    ""id"": ""970b6d1d-fa21-4b77-b252-d3c7250754be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.01)""
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""0360fd5b-bac4-4d59-a40d-60ef0beb7003"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.01)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""MovementKeys"",
                    ""id"": ""1b6ffd84-5298-4958-ae08-13d15ba02513"",
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
                    ""id"": ""d6b7519a-9038-4a2c-94ed-542f56d14391"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2469c143-9d18-41db-9d24-4dd2cbb01cb9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4c0d1997-5e1d-4c73-86e4-ef678e9112e1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3230e18b-e911-4319-8e36-81f62cc7ab07"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f95fff97-1d33-47b7-8181-f578b40bc173"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdf90225-33a9-4090-9166-4f1ce023db0a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""285495a2-38a4-4af9-b5f3-fcdac1cb7be9"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc4c2a99-2796-436c-b9f0-87e4001366d4"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55f92344-92a1-4f95-9268-346061a0baa7"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33dbefbf-a2c2-432b-a2c6-19aa788d4253"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2febbb0b-5790-4365-8cf0-d2e93167c4fc"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c361317d-8170-4e50-925b-c3c90b507687"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""HighlightObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7fd594bb-1b1f-43a3-8e6b-91e798b61610"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""rotateObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19a91bd2-2dea-43de-88af-584f85596ff9"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be5516b5-dad0-42ae-b353-e18aa0ee2b45"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Torch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e596b079-dd62-40b2-a140-110c3058bc7e"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Torch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31268c2d-5a25-46e7-a66b-d331b286cd12"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""BreakingIce"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53987c23-bbc0-4b59-9159-1928940e5712"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_OpenInventory = m_Player.FindAction("OpenInventory", throwIfNotFound: true);
        m_Player_PickUp = m_Player.FindAction("PickUp", throwIfNotFound: true);
        m_Player_HighlightObject = m_Player.FindAction("HighlightObject", throwIfNotFound: true);
        m_Player_rotateObject = m_Player.FindAction("rotateObject", throwIfNotFound: true);
        m_Player_PlayPause = m_Player.FindAction("PlayPause", throwIfNotFound: true);
        m_Player_Torch = m_Player.FindAction("Torch", throwIfNotFound: true);
        m_Player_BreakingIce = m_Player.FindAction("BreakingIce", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_OpenInventory;
    private readonly InputAction m_Player_PickUp;
    private readonly InputAction m_Player_HighlightObject;
    private readonly InputAction m_Player_rotateObject;
    private readonly InputAction m_Player_PlayPause;
    private readonly InputAction m_Player_Torch;
    private readonly InputAction m_Player_BreakingIce;
    private readonly InputAction m_Player_Run;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @OpenInventory => m_Wrapper.m_Player_OpenInventory;
        public InputAction @PickUp => m_Wrapper.m_Player_PickUp;
        public InputAction @HighlightObject => m_Wrapper.m_Player_HighlightObject;
        public InputAction @rotateObject => m_Wrapper.m_Player_rotateObject;
        public InputAction @PlayPause => m_Wrapper.m_Player_PlayPause;
        public InputAction @Torch => m_Wrapper.m_Player_Torch;
        public InputAction @BreakingIce => m_Wrapper.m_Player_BreakingIce;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @OpenInventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @OpenInventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenInventory;
                @PickUp.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                @PickUp.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                @PickUp.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickUp;
                @HighlightObject.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHighlightObject;
                @HighlightObject.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHighlightObject;
                @HighlightObject.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHighlightObject;
                @rotateObject.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateObject;
                @rotateObject.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateObject;
                @rotateObject.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotateObject;
                @PlayPause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayPause;
                @PlayPause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayPause;
                @PlayPause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlayPause;
                @Torch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTorch;
                @Torch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTorch;
                @Torch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTorch;
                @BreakingIce.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakingIce;
                @BreakingIce.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakingIce;
                @BreakingIce.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakingIce;
                @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @OpenInventory.started += instance.OnOpenInventory;
                @OpenInventory.performed += instance.OnOpenInventory;
                @OpenInventory.canceled += instance.OnOpenInventory;
                @PickUp.started += instance.OnPickUp;
                @PickUp.performed += instance.OnPickUp;
                @PickUp.canceled += instance.OnPickUp;
                @HighlightObject.started += instance.OnHighlightObject;
                @HighlightObject.performed += instance.OnHighlightObject;
                @HighlightObject.canceled += instance.OnHighlightObject;
                @rotateObject.started += instance.OnRotateObject;
                @rotateObject.performed += instance.OnRotateObject;
                @rotateObject.canceled += instance.OnRotateObject;
                @PlayPause.started += instance.OnPlayPause;
                @PlayPause.performed += instance.OnPlayPause;
                @PlayPause.canceled += instance.OnPlayPause;
                @Torch.started += instance.OnTorch;
                @Torch.performed += instance.OnTorch;
                @Torch.canceled += instance.OnTorch;
                @BreakingIce.started += instance.OnBreakingIce;
                @BreakingIce.performed += instance.OnBreakingIce;
                @BreakingIce.canceled += instance.OnBreakingIce;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnOpenInventory(InputAction.CallbackContext context);
        void OnPickUp(InputAction.CallbackContext context);
        void OnHighlightObject(InputAction.CallbackContext context);
        void OnRotateObject(InputAction.CallbackContext context);
        void OnPlayPause(InputAction.CallbackContext context);
        void OnTorch(InputAction.CallbackContext context);
        void OnBreakingIce(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
}
