// GENERATED AUTOMATICALLY FROM 'Assets/Controler.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controler : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controler()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controler"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""1cd547d4-915d-4eb5-8ee2-9455f7e797a9"",
            ""actions"": [
                {
                    ""name"": ""Mouvement"",
                    ""type"": ""Value"",
                    ""id"": ""3b20a15f-882a-412e-8e86-7018bd8f5813"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""0c602d8e-517c-4660-b9d9-0e5f95760081"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""dd8f46c9-f1f4-4f32-8c3f-04a441cb0557"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Running"",
                    ""type"": ""Value"",
                    ""id"": ""7b9ab7c7-f80c-4139-a8fa-fb7f4fe5e1c5"",
                    ""expectedControlType"": ""DiscreteButton"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cam Anchor"",
                    ""type"": ""Button"",
                    ""id"": ""12416e99-0068-4b4a-894b-7fb08836d9fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""7cc398f3-d250-49b3-9e4f-2b9a84e96c85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""246ebf58-b439-4216-9516-0da24a6944c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""132e227a-9b65-412e-93a3-7c65892df784"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BoostDev"",
                    ""type"": ""Button"",
                    ""id"": ""067e7fe4-ec54-4a23-a952-34d2955ba51e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2f3105b5-25e2-414f-9eae-a7982dcc78fb"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6ff6d28-85a8-4ca1-95bf-28a5f4ba0098"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2a3b414-bed5-4cb0-9a28-dca8cce77de8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ab1d6c9-56ce-40a3-81a8-76188b19b3c3"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc8f1e8f-31ca-4122-9450-df7b47b280ca"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bb01f47-d79b-47fc-805e-33550cc6ab62"",
                    ""path"": ""<Touchscreen>/touch1/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mobile"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9911caf3-1b2e-49f4-89d1-d80b82fd344c"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=8,y=8)"",
                    ""groups"": ""Manettes"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71d2b73f-6845-4d64-8f6e-e49009c6a7b0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f4608b2-9a67-4109-b8eb-8e81ac47548c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1d76b694-209b-49a8-ac1d-815fccd2d7cd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouvement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b2cb815a-fe3a-4395-8cd5-12dfb0e057bc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4e0a7acf-ce75-486b-a72c-f60cd9736dcb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2319bf30-efdc-4be0-9fbf-34bc29fe6c57"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bbc7b148-bed8-483d-9038-6e1ebd41ad46"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5d4b3059-889e-4514-a1d1-3b66f449b79c"",
                    ""path"": ""<Touchscreen>/touch0/delta"",
                    ""interactions"": ""Press"",
                    ""processors"": ""ScaleVector2(x=0.1,y=0.1)"",
                    ""groups"": ""Mobile"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ade8fcd-dcee-4159-be49-5a6b8275e394"",
                    ""path"": ""<DualShockGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Mouvement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2b041fa-0c83-47e5-9cd2-411c41387cce"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Running"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43ab547b-73ed-4c99-a734-96f146df5ee9"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Running"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1da3beb4-467a-47a5-ac08-bf0fc61c8c66"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b527eb6-ea3f-4af9-92d8-2fb59a5efdb3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f9a980f-540c-49dd-87ad-b7dcab359a55"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Cam Anchor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32fc7a27-9c6e-4e0d-8355-9c5d81fc7104"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Cam Anchor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1eb8a5cc-43c9-46c0-bd91-6ab952ac057d"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""BoostDev"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""45bdfa37-4149-4b76-81cc-0d00e9840abb"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""0ae264da-8868-4b69-bec0-2a7438610a70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f34fd73c-9d4a-4ff0-9a88-9f1937da5f7e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dba17f2e-f578-4def-a28d-cbec343ecc3e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Manettes"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Soutenance"",
            ""id"": ""55fc19c4-136e-456d-9fef-d48ca7d0206d"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""986aa32d-6850-4861-9d2b-2fb3f9642e07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6653953c-d629-470b-9c6f-c408eaf957f5"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mobile"",
            ""bindingGroup"": ""Mobile"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<AndroidGamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Manettes"",
            ""bindingGroup"": ""Manettes"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Mouvement = m_Player.FindAction("Mouvement", throwIfNotFound: true);
        m_Player_Camera = m_Player.FindAction("Camera", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Running = m_Player.FindAction("Running", throwIfNotFound: true);
        m_Player_CamAnchor = m_Player.FindAction("Cam Anchor", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_BoostDev = m_Player.FindAction("BoostDev", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Pause = m_Menu.FindAction("Pause", throwIfNotFound: true);
        // Soutenance
        m_Soutenance = asset.FindActionMap("Soutenance", throwIfNotFound: true);
        m_Soutenance_Newaction = m_Soutenance.FindAction("New action", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Mouvement;
    private readonly InputAction m_Player_Camera;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Running;
    private readonly InputAction m_Player_CamAnchor;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_BoostDev;
    public struct PlayerActions
    {
        private @Controler m_Wrapper;
        public PlayerActions(@Controler wrapper) { m_Wrapper = wrapper; }
        public InputAction @Mouvement => m_Wrapper.m_Player_Mouvement;
        public InputAction @Camera => m_Wrapper.m_Player_Camera;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Running => m_Wrapper.m_Player_Running;
        public InputAction @CamAnchor => m_Wrapper.m_Player_CamAnchor;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @BoostDev => m_Wrapper.m_Player_BoostDev;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Mouvement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouvement;
                @Mouvement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouvement;
                @Mouvement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouvement;
                @Camera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamera;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Running.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunning;
                @Running.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunning;
                @Running.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRunning;
                @CamAnchor.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamAnchor;
                @CamAnchor.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamAnchor;
                @CamAnchor.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCamAnchor;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @BoostDev.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoostDev;
                @BoostDev.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoostDev;
                @BoostDev.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoostDev;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Mouvement.started += instance.OnMouvement;
                @Mouvement.performed += instance.OnMouvement;
                @Mouvement.canceled += instance.OnMouvement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Running.started += instance.OnRunning;
                @Running.performed += instance.OnRunning;
                @Running.canceled += instance.OnRunning;
                @CamAnchor.started += instance.OnCamAnchor;
                @CamAnchor.performed += instance.OnCamAnchor;
                @CamAnchor.canceled += instance.OnCamAnchor;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @BoostDev.started += instance.OnBoostDev;
                @BoostDev.performed += instance.OnBoostDev;
                @BoostDev.canceled += instance.OnBoostDev;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Pause;
    public struct MenuActions
    {
        private @Controler m_Wrapper;
        public MenuActions(@Controler wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Menu_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // Soutenance
    private readonly InputActionMap m_Soutenance;
    private ISoutenanceActions m_SoutenanceActionsCallbackInterface;
    private readonly InputAction m_Soutenance_Newaction;
    public struct SoutenanceActions
    {
        private @Controler m_Wrapper;
        public SoutenanceActions(@Controler wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Soutenance_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Soutenance; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SoutenanceActions set) { return set.Get(); }
        public void SetCallbacks(ISoutenanceActions instance)
        {
            if (m_Wrapper.m_SoutenanceActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_SoutenanceActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_SoutenanceActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_SoutenanceActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_SoutenanceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public SoutenanceActions @Soutenance => new SoutenanceActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_MobileSchemeIndex = -1;
    public InputControlScheme MobileScheme
    {
        get
        {
            if (m_MobileSchemeIndex == -1) m_MobileSchemeIndex = asset.FindControlSchemeIndex("Mobile");
            return asset.controlSchemes[m_MobileSchemeIndex];
        }
    }
    private int m_ManettesSchemeIndex = -1;
    public InputControlScheme ManettesScheme
    {
        get
        {
            if (m_ManettesSchemeIndex == -1) m_ManettesSchemeIndex = asset.FindControlSchemeIndex("Manettes");
            return asset.controlSchemes[m_ManettesSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMouvement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRunning(InputAction.CallbackContext context);
        void OnCamAnchor(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnBoostDev(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
    public interface ISoutenanceActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
