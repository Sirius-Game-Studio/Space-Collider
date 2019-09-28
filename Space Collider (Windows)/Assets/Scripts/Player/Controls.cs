// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Controls : IInputActionCollection
{
    private InputActionAsset asset;
    public Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""9f5629a4-bc86-4dab-93b6-79ee902fb149"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ac6d7038-ed88-4cac-b595-0ce787bae89c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""db8af2a9-8508-4a3b-8955-8bf488a393c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b4092350-a063-45fb-8466-bc621cf49640"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5903901c-0c78-4b82-9389-f0ac6e15bb39"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""781fc239-1668-497d-ab4a-3b947f6f813e"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14d1c4d8-aadd-4530-9ba6-4cd6e432c319"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""6be0cb67-0576-4d0e-abda-a87b83758ca8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""dbf1e9de-84fb-47d1-8dec-dfc997bdf5b5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b94e9085-e0da-4f18-9857-cfb7e8245c24"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""538d6294-435f-42ab-91e2-53f8c8e8bcfb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4e046e9b-2311-4712-8eab-a3bb5032dded"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""0d56d973-b8e0-4db7-af75-0f6a34d1bd41"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2314b140-31c4-49d9-ad32-2e3126ea36d4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f3fdf30a-5e7e-40ad-a77c-55e216ab1e32"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""680dd077-eab6-4227-a4ed-eae189dff45e"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dc831278-7ee4-47ef-aa28-a8c22004c95d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Analog"",
                    ""id"": ""79286962-408d-42f8-9991-174ee10681eb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b4a8b9a8-8c29-46d1-a620-3c8c89c46ed7"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""09db97a4-bb5f-4f2a-8716-d42fe649973f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6cf39ad5-35c0-42dd-8b97-765420cbb769"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""012b44db-5c03-4d5e-be8b-b23d388b049e"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""2507e08e-00a0-4b5d-bd9a-128c4b4c502c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""12cf15ca-0824-490b-9cd4-fe877e52994e"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bb98791f-a55f-46c5-ab03-c2b0c6f14663"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cf84a52a-6325-4828-a040-b3ffb0b38f61"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7575f831-d21f-44f4-b50f-fdc4766e0d64"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""0098ec7f-806e-4c5c-993d-acf8df03beca"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""57008016-267b-446e-92a1-53f8acced8ad"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""26761fbb-b362-4d94-bf53-79b4a25eee34"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a7beea3b-9911-4ade-806e-c7b0d667357c"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""be9af3a1-195f-400a-b8e0-24cf2d2fce61"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Gameplay"",
            ""id"": ""fa3bc070-31dd-4bca-87d2-0a42c80a7889"",
            ""actions"": [
                {
                    ""name"": ""Fullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""f35ada1e-e17f-4d74-98d6-3301d0a87149"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""75936f3d-293c-4cfe-8b64-0fc7ac89e666"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Resume"",
                    ""type"": ""Button"",
                    ""id"": ""b347855a-b4d0-4103-84ba-3cabc3ce7638"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""640e2732-a080-4111-b213-9f1be1181560"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5865f171-9870-4b5a-972f-3063c41de6eb"",
                    ""path"": ""<Keyboard>/f11"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Fullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b145fb7-a4d1-458d-82fc-a0d321c812bc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""509e1651-66c5-46e5-8fbd-369de4e1c31e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""817eedbe-8e2b-4230-b7bd-7609ff87be3c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Resume"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4db20af-453e-4545-9586-b5b9db1bdd71"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""ae75ae21-49d0-4b8c-b4e5-2f67a02d55c4"",
            ""actions"": [
                {
                    ""name"": ""ClearHighScores"",
                    ""type"": ""Button"",
                    ""id"": ""ae97a100-87eb-455f-a0ba-399a4159f02b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CloseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""9c831bc1-5b26-47e1-9878-328ab65c0aa5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""62573ea4-2fcb-421e-8b7b-241109429663"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ClearHighScores"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6463830c-802c-4dc2-bac7-77102fda1ae7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CloseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df511a31-de65-4670-a83f-be289923daf8"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CloseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Sound"",
            ""id"": ""6d17b40a-b39d-4407-bb73-ff7246bc8258"",
            ""actions"": [
                {
                    ""name"": ""LowerSound"",
                    ""type"": ""Button"",
                    ""id"": ""d407e0a4-8fe8-45e4-a949-c7f4556dd7e2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LowerMusic"",
                    ""type"": ""Button"",
                    ""id"": ""3dbf0c0f-8dd5-4a0e-89a4-1a7bb838cae5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IncreaseSound"",
                    ""type"": ""Button"",
                    ""id"": ""e1f8988c-0816-4c3e-b1cf-ed8a3522999e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IncreaseMusic"",
                    ""type"": ""Button"",
                    ""id"": ""3fd865eb-9ba5-4c33-a065-e59e46cf7062"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""82c9d527-07e3-4f63-add9-d4adcc49024b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LowerSound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65e80a46-d425-4a5d-b725-4b492c28eb41"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LowerMusic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d633747-6384-4eed-995a-c7d2909bd8d1"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""IncreaseSound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a16b0de9-415d-46a5-a9eb-7bde9301776a"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""IncreaseMusic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
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
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Fullscreen = m_Gameplay.FindAction("Fullscreen", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_Resume = m_Gameplay.FindAction("Resume", throwIfNotFound: true);
        m_Gameplay_Restart = m_Gameplay.FindAction("Restart", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_ClearHighScores = m_Menu.FindAction("ClearHighScores", throwIfNotFound: true);
        m_Menu_CloseMenu = m_Menu.FindAction("CloseMenu", throwIfNotFound: true);
        // Sound
        m_Sound = asset.FindActionMap("Sound", throwIfNotFound: true);
        m_Sound_LowerSound = m_Sound.FindAction("LowerSound", throwIfNotFound: true);
        m_Sound_LowerMusic = m_Sound.FindAction("LowerMusic", throwIfNotFound: true);
        m_Sound_IncreaseSound = m_Sound.FindAction("IncreaseSound", throwIfNotFound: true);
        m_Sound_IncreaseMusic = m_Sound.FindAction("IncreaseMusic", throwIfNotFound: true);
    }

    ~Controls()
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
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Fire;
    public struct PlayerActions
    {
        private Controls m_Wrapper;
        public PlayerActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Fullscreen;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_Resume;
    private readonly InputAction m_Gameplay_Restart;
    public struct GameplayActions
    {
        private Controls m_Wrapper;
        public GameplayActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fullscreen => m_Wrapper.m_Gameplay_Fullscreen;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @Resume => m_Wrapper.m_Gameplay_Resume;
        public InputAction @Restart => m_Wrapper.m_Gameplay_Restart;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                Fullscreen.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFullscreen;
                Fullscreen.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFullscreen;
                Fullscreen.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFullscreen;
                Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                Resume.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResume;
                Resume.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResume;
                Resume.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResume;
                Restart.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
                Restart.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
                Restart.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRestart;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                Fullscreen.started += instance.OnFullscreen;
                Fullscreen.performed += instance.OnFullscreen;
                Fullscreen.canceled += instance.OnFullscreen;
                Pause.started += instance.OnPause;
                Pause.performed += instance.OnPause;
                Pause.canceled += instance.OnPause;
                Resume.started += instance.OnResume;
                Resume.performed += instance.OnResume;
                Resume.canceled += instance.OnResume;
                Restart.started += instance.OnRestart;
                Restart.performed += instance.OnRestart;
                Restart.canceled += instance.OnRestart;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_ClearHighScores;
    private readonly InputAction m_Menu_CloseMenu;
    public struct MenuActions
    {
        private Controls m_Wrapper;
        public MenuActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ClearHighScores => m_Wrapper.m_Menu_ClearHighScores;
        public InputAction @CloseMenu => m_Wrapper.m_Menu_CloseMenu;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                ClearHighScores.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnClearHighScores;
                ClearHighScores.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnClearHighScores;
                ClearHighScores.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnClearHighScores;
                CloseMenu.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCloseMenu;
                CloseMenu.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCloseMenu;
                CloseMenu.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCloseMenu;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                ClearHighScores.started += instance.OnClearHighScores;
                ClearHighScores.performed += instance.OnClearHighScores;
                ClearHighScores.canceled += instance.OnClearHighScores;
                CloseMenu.started += instance.OnCloseMenu;
                CloseMenu.performed += instance.OnCloseMenu;
                CloseMenu.canceled += instance.OnCloseMenu;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // Sound
    private readonly InputActionMap m_Sound;
    private ISoundActions m_SoundActionsCallbackInterface;
    private readonly InputAction m_Sound_LowerSound;
    private readonly InputAction m_Sound_LowerMusic;
    private readonly InputAction m_Sound_IncreaseSound;
    private readonly InputAction m_Sound_IncreaseMusic;
    public struct SoundActions
    {
        private Controls m_Wrapper;
        public SoundActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LowerSound => m_Wrapper.m_Sound_LowerSound;
        public InputAction @LowerMusic => m_Wrapper.m_Sound_LowerMusic;
        public InputAction @IncreaseSound => m_Wrapper.m_Sound_IncreaseSound;
        public InputAction @IncreaseMusic => m_Wrapper.m_Sound_IncreaseMusic;
        public InputActionMap Get() { return m_Wrapper.m_Sound; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SoundActions set) { return set.Get(); }
        public void SetCallbacks(ISoundActions instance)
        {
            if (m_Wrapper.m_SoundActionsCallbackInterface != null)
            {
                LowerSound.started -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerSound;
                LowerSound.performed -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerSound;
                LowerSound.canceled -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerSound;
                LowerMusic.started -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerMusic;
                LowerMusic.performed -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerMusic;
                LowerMusic.canceled -= m_Wrapper.m_SoundActionsCallbackInterface.OnLowerMusic;
                IncreaseSound.started -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseSound;
                IncreaseSound.performed -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseSound;
                IncreaseSound.canceled -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseSound;
                IncreaseMusic.started -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseMusic;
                IncreaseMusic.performed -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseMusic;
                IncreaseMusic.canceled -= m_Wrapper.m_SoundActionsCallbackInterface.OnIncreaseMusic;
            }
            m_Wrapper.m_SoundActionsCallbackInterface = instance;
            if (instance != null)
            {
                LowerSound.started += instance.OnLowerSound;
                LowerSound.performed += instance.OnLowerSound;
                LowerSound.canceled += instance.OnLowerSound;
                LowerMusic.started += instance.OnLowerMusic;
                LowerMusic.performed += instance.OnLowerMusic;
                LowerMusic.canceled += instance.OnLowerMusic;
                IncreaseSound.started += instance.OnIncreaseSound;
                IncreaseSound.performed += instance.OnIncreaseSound;
                IncreaseSound.canceled += instance.OnIncreaseSound;
                IncreaseMusic.started += instance.OnIncreaseMusic;
                IncreaseMusic.performed += instance.OnIncreaseMusic;
                IncreaseMusic.canceled += instance.OnIncreaseMusic;
            }
        }
    }
    public SoundActions @Sound => new SoundActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
    public interface IGameplayActions
    {
        void OnFullscreen(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnResume(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnClearHighScores(InputAction.CallbackContext context);
        void OnCloseMenu(InputAction.CallbackContext context);
    }
    public interface ISoundActions
    {
        void OnLowerSound(InputAction.CallbackContext context);
        void OnLowerMusic(InputAction.CallbackContext context);
        void OnIncreaseSound(InputAction.CallbackContext context);
        void OnIncreaseMusic(InputAction.CallbackContext context);
    }
}
