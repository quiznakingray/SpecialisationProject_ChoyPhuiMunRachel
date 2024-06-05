using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerObserver
{
    [SerializeField] Transform thirdPersonCamera;
    [SerializeField] Transform firstPersonCamera;

    [SerializeField] private PlayerManager player;

    //movment keys
    float horizontal;
    float vertical;

    // player & camera directions
    Vector3 forwardRelative;
    Vector3 rightRelative;
    Vector3 cameraRelativeMovement;

    //jump booleans
    bool readyToJump;
    bool isGrounded;

    // crouch booleans
    bool readyToCrouch;

    // rb and coliders
    Rigidbody rb;
    CapsuleCollider playerCollider;
    
    //player heights
    private float playerHeight;
    private float playerHeightOffset;

    // timer
    private float transitionTimer;

    // velocity
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        readyToJump = true;

        readyToCrouch = true;
        playerHeight = playerCollider.height;
        playerHeightOffset = playerCollider.center.y;

        player.StandingHeight = playerCollider.height;
        player.StandingHeightOffset = playerCollider.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is on the ground
        CheckGroundedStatus();

        if (isGrounded)
        {
            rb.drag = PlayerManager.Instance.groundDrag;
            PlayerResetJump();
        }
        else
        {
            rb.drag = 0;
        }

        lerpHeight(playerHeight, 0.4f);
        lerpHeightCenterOffset(playerHeightOffset, 0.4f);

        // debugging

        Vector3 rayPosition = new Vector3(transform.position.x, (float)(transform.position.y + playerCollider.height * 0.5), transform.position.z);
        Vector3 camForward = thirdPersonCamera.forward;
        camForward.y = 0;
        Debug.DrawRay(rayPosition, camForward, Color.green);
        Vector3 playerForward = transform.forward;
        Debug.DrawRay(rayPosition, playerForward, Color.cyan);


        Debug.Log(playerCollider.height);
    }
    private void CheckGroundedStatus()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;
        float rayLength = PlayerManager.Instance.StandingHeight * 0.5f + 0.1f;

        // Log the height to ensure it's correct
        Debug.Log("Collider Height: " + PlayerManager.Instance.StandingHeight);

        // Perform the raycast
        isGrounded = Physics.Raycast(rayOrigin, rayDirection, rayLength, PlayerManager.Instance.groundLayer);

        // Draw the ray for debugging purposes
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);

        // Log the grounded status (optional)
        Debug.Log("Is Grounded: " + isGrounded);
    }
    private void Walk()
    {
        velocity = PlayerManager.Instance.walkingSpeed;
        MovePlayer();
    }
    
    private void Running()
    {
        velocity = PlayerManager.Instance.runningSpeed;
        MovePlayer();
    }
    private void MovePlayer()
    {
        horizontal = InputManager.Instance.horizontal;
        vertical = InputManager.Instance.vertical;

        Vector3 rayPosition = new Vector3(transform.position.x, (float)(transform.position.y + playerCollider.height * 0.5), transform.position.z);

        Vector3 camForward;
        Vector3 camRight;
        if (PlayerManager.Instance.isFirstPersonView)
        {
            camForward = firstPersonCamera.forward;
            camRight = firstPersonCamera.right;
        }
        else
        {
            camForward = thirdPersonCamera.forward;
            camRight = thirdPersonCamera.right;
        }

        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        forwardRelative = vertical * camForward;
        rightRelative = horizontal * camRight;

        cameraRelativeMovement = forwardRelative + rightRelative;
        Vector3 direction = cameraRelativeMovement.normalized;

        rb.velocity = new Vector3(direction.x * velocity, rb.velocity.y, direction.z * velocity);

        if (direction.magnitude > 0)
        {

            // rotate character accrding to movement
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, PlayerManager.Instance.playerMovementLerpDuration);

            transform.rotation = Quaternion.Euler(0, angle, 0);

        }
    }

    private void PlayerJump()
    {
        if (readyToJump && isGrounded)
        {
            //animator.SetBool("isJumping", true);
            readyToJump = false;
            rb.AddForce(transform.up *  PlayerManager.Instance.jumpForce, ForceMode.Impulse);
        }
    }

    private void PlayerResetJump()
    {
        if (isGrounded)
        {
            readyToJump = true;
        }
    }

#pragma 
    private void Crouch()
    {
        if (readyToCrouch && isGrounded) // see if can crouch-jump
        {
            readyToCrouch = false;
            //animator.SetBool("isCrouch", true);

        }

    }
    private void Crouching()
    {
        if (!readyToCrouch)
        {
            playerHeight = PlayerManager.Instance.CrouchHeight;
            playerHeightOffset = PlayerManager.Instance.crouchCenterOffset;
            //animator.SetBool("isCrouching", true);
            lerpToTargetVelocity(PlayerManager.Instance.crouchingSpeed, 0.5f);

        }
    }
    private void ResetCrouch()
    {
        if (!readyToCrouch)
        {
            playerHeight = PlayerManager.Instance.StandingHeight;
            playerHeightOffset = PlayerManager.Instance.StandingHeightOffset;
            readyToCrouch = true;
            //animator.SetBool("isCrouch", false);
            //animator.SetBool("isCrouching", false);
        }

    }

    private void lerpToTargetVelocity(float targetVelocity, float lerpDuration)
    {
        transitionTimer = 0;
        lerpVelocity(targetVelocity, lerpDuration);
    }

    private void lerpVelocity(float lerpValue, float lerpDuration)
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / lerpDuration);
        velocity = Mathf.Lerp(velocity, lerpValue, t);
        if (transitionTimer >= lerpDuration)
        {
            return;
        }
    }

    private void lerpHeight(float lerpValue, float lerpDuration)
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / lerpDuration);
        playerCollider.height = Mathf.Lerp(playerCollider.height, lerpValue, t);
        if (transitionTimer >= lerpDuration)
        {
            return;
        }
    }
    private void lerpHeightCenterOffset(float lerpValue, float lerpDuration)
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / lerpDuration);
        playerCollider.center = new Vector3(playerCollider.center.x, Mathf.Lerp(playerCollider.center.y, lerpValue, t), playerCollider.center.z);
        if (transitionTimer >= lerpDuration)
        {
            return;
        }
    }
    public void OnNotify(PlayerActions action)
    {
        switch (action)
        {
            case PlayerActions.Walk:
                Walk();
                break;

            case PlayerActions.Sprinting:
                Running();
                break;

            case PlayerActions.Jump:
                PlayerJump();
                break;

            case PlayerActions.Jumped:
                PlayerResetJump();
                break;

            case PlayerActions.Crouch:
                Crouch();
                break;
                
            case PlayerActions.Crouching:
                Crouching();
                break;
                
            case PlayerActions.Crouched:
                ResetCrouch();
                break;
        }
        
        

    }

    private void OnEnable()
    {
        player.AddObservers(this);
    }

    private void OnDisable()
    {
        player.RemoveObserver(this);
    }
}
