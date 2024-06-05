using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [HideInInspector] public float mouseScrollWheel;

    [HideInInspector] public bool No1Button;
    [HideInInspector] public bool No2Button;

    [HideInInspector] public bool No5Button;

    //-------------------------------- NEW ---
    //mouse movement
    [HideInInspector] public float MouseMovementX;
    [HideInInspector] public float MouseMovementY;
    // player movement

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;

    [HideInInspector] public bool sprint;
    [HideInInspector] public bool sprinting;
    [HideInInspector] public bool sprinted;

    [HideInInspector] public bool jump;
    [HideInInspector] public bool jumped;

    [HideInInspector] public bool crouch;
    [HideInInspector] public bool crouching;
    [HideInInspector] public bool crouched;

    // camera movement
    [HideInInspector] public bool toggleFOV;
    [HideInInspector] public bool toggleADS;

    // battle 
    [HideInInspector] public bool attack;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool attacked;

    //hotkeys
    [HideInInspector] public bool backpackHotKey;

    [HideInInspector] public bool testingButton;

    // conditions 
    [HideInInspector] public bool MovementKeysLocked;
    [HideInInspector] public bool CameraKeysLocked;
    [HideInInspector] public bool BattleKeysLocked;
    [HideInInspector] public bool HotKeysLocked;


    // ---------------------


    [Header("Movement")]
    [SerializeField] KeyCode ForwardMovementKey;
    [SerializeField] KeyCode LeftMovementKey;
    [SerializeField] KeyCode BackwardMovementKey;
    [SerializeField] KeyCode RightMovementKey;

    [SerializeField] KeyCode SprintingKey;

    [SerializeField] KeyCode CrouchingKey;

    [SerializeField] KeyCode JumpingKey;

    [Header("FOV")]

    [SerializeField] KeyCode ChangeFOVKey;
    [SerializeField] KeyCode ADSKey;

    [Header("Battle")]
    [SerializeField] KeyCode AttackKey;


    [Header("Hot Keys")]
    [SerializeField] KeyCode BackpackHotKey;

    [Header("Testing")]

    [SerializeField] KeyCode TestingKey;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        MovementKeysLocked = false;
        CameraKeysLocked = false;
        BattleKeysLocked = false;
        HotKeysLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        // mouse movement
        MouseMovementX = Input.GetAxis("Mouse X");
        MouseMovementY = Input.GetAxis("Mouse Y");

        No1Button = Input.GetKeyUp(KeyCode.Alpha1);
        No2Button = Input.GetKeyUp(KeyCode.Alpha2);

        No5Button = Input.GetKeyDown(KeyCode.Alpha5);

        testingButton = Input.GetKeyDown(TestingKey);


        if (!MovementKeysLocked)
        {
            MovementKeys();
        }
        if (!CameraKeysLocked)
        {
            CameraKeys();
        }
        if(!BattleKeysLocked)
        {
            BattleKeys();
        }

        if (!HotKeysLocked)
        {
            HotKeys();
        }

        if (testingButton)
        {
            //MovementKeysLocked = !MovementKeysLocked;
        }
    }

    void MovementKeys()
    {
        //walking
        horizontal = ((Input.GetKey(RightMovementKey) ? 1 : 0) - (Input.GetKey(LeftMovementKey) ? 1 : 0));
        vertical = ((Input.GetKey(ForwardMovementKey) ? 1 : 0) - (Input.GetKey(BackwardMovementKey) ? 1 : 0));

        // jumping
        jump = Input.GetKeyDown(JumpingKey);
        jumped = Input.GetKeyUp(JumpingKey);

        // crouching
        crouch = Input.GetKeyDown(CrouchingKey);
        crouching = Input.GetKey(CrouchingKey);
        crouched = Input.GetKeyUp(CrouchingKey);

        // sprinting
        sprint = Input.GetKeyDown(SprintingKey);
        sprinting = Input.GetKey(SprintingKey);
        sprinted = Input.GetKeyUp(SprintingKey);
    }

    void CameraKeys()
    {
        // change FOV
        toggleFOV = Input.GetKeyDown(ChangeFOVKey);
        toggleADS = Input.GetKeyDown(ADSKey);
        mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
    }

    void BattleKeys()
    {
        attack = Input.GetKeyDown(AttackKey);
        attacking = Input.GetKey(AttackKey);
        attacked = Input.GetKeyUp(AttackKey);
    }
    
    void HotKeys()
    {
        backpackHotKey = Input.GetKeyUp(BackpackHotKey);
    }
}