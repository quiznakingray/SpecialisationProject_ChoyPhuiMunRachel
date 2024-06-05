using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions
{
    //player action
    Idle,
    Walk,
    Sprint,
    Sprinting,
    Sprinted,
    Crouch,
    Crouching,
    Crouched,
    Jump,
    Jumped,

    // camera actions
    CameraZoomInOut,
    ToggleFOV,
    ToggleADS,

    // battle actions
    BasicAttack,
    ChargedAttack,
    Skill01,
    Skill02,
    Ultimate,

    // weapon change
    EquipWeapon01,
    EquipWeapon02,


}

public class PlayerManager : PlayerSubject
{
    public static PlayerManager Instance;
    // Start is called before the first frame update

    [Header("Player Speeds")]
    public float walkingSpeed;
    public float runningSpeed;
    public float crouchingSpeed;

    [Header("Player Jump Variables")]
    public float jumpForce;
    
    [Header("Player Crouch Variables")]
    public float crouchCenterOffset;
    
    [Header("Set Player Heights & Offsets")]
    public float CrouchHeight;
    public float StandingHeight;
    public float CrouchHeightOffset;
    public float StandingHeightOffset;

    [Header("Game Object Drags")]
    public float groundDrag;

    [Header("Game Object Layers")]
    public LayerMask groundLayer;

    [Header("Booleans")]
    public bool isFirstPersonView;
    public bool ADS;
    public bool isDoingAction;

    [Header("Player Movement")]
    public float playerMovementLerpDuration;

    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        isFirstPersonView = false;
        ADS = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputManager.Instance.MovementKeysLocked)
        {
            HandleMovement();
            HandleCrouching();
            if (!ADS)
            {
                HandleJumping();

            }
        }

        if (!InputManager.Instance.CameraKeysLocked)
        {
            HandleCameraActions();

        }

        if (!InputManager.Instance.HotKeysLocked)
        {
            HandeHotKeys();
        }

    }

    private void HandleCameraActions()
    {
        if (InputManager.Instance.toggleFOV)
        {
            NotifyObservers(PlayerActions.ToggleFOV);
        }

        if (InputManager.Instance.toggleADS)
        {
            NotifyObservers(PlayerActions.ToggleADS);
        }

        if (!ADS && InputManager.Instance.mouseScrollWheel != 0f)
        {
            NotifyObservers(PlayerActions.CameraZoomInOut);
        }
    }
    void HandleMovement()
    {
        if (InputManager.Instance.horizontal != 0 || InputManager.Instance.vertical != 0)
        {
            if (InputManager.Instance.sprint)
            {
                NotifyObservers(PlayerActions.Sprint);
            }
            else if (InputManager.Instance.sprinting)
            {
                NotifyObservers(PlayerActions.Sprinting);
            }
            else if (InputManager.Instance.sprinted)
            {
                NotifyObservers(PlayerActions.Sprinted);
            }
            else
            {
                NotifyObservers(PlayerActions.Walk);
            }
        }
        else
        {
            NotifyObservers(PlayerActions.Idle);
        }

    }

    void HandleJumping()
    {
        if (InputManager.Instance.jump)
        {
            NotifyObservers(PlayerActions.Jump);
        }
        else if (InputManager.Instance.jumped)
        {
            NotifyObservers(PlayerActions.Jumped);
        }
        else
        {
            isDoingAction = false;
        }
        isDoingAction = true;
    }

    void HandleCrouching()  
    {
        if (InputManager.Instance.crouch)
        {
            NotifyObservers(PlayerActions.Crouch);
        }
        else if (InputManager.Instance.crouching)
        {
            NotifyObservers(PlayerActions.Crouching);
        }
        else if (InputManager.Instance.crouched)
        {
            NotifyObservers(PlayerActions.Crouched);
        }
        else
        {
            isDoingAction = false;
        }

        isDoingAction = true;
    }

    void HandeHotKeys()
    {
        if (InputManager.Instance.backpackHotKey)
        {
            UIManager.Instance.OpenBackpack();
        }
    }
    public bool getIsDoingAction()
    {
        return isDoingAction;
    }

}
