using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform playerGO;
    [SerializeField] private Transform target;
    [SerializeField] private Transform overShoulderCamTarget;
    [SerializeField] private Transform headLookAt;
    [SerializeField, Range(1, 5)] private float distance = 3;
    [SerializeField, Range(0, 5)] private float minDistance = 1;
    [SerializeField] private float heightOffset = 0.5f;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField, Range(0, 90)] private float cameraAngleUpLimit = 30.0f;
    [SerializeField, Range(0, 90)] private float cameraAngleDownLimit = 30.0f;
    [SerializeField, Range(0, 90)] private float aimCameraAngleUpLimit = 30.0f;
    [SerializeField, Range(0, 90)] private float aimCameraAngleDownLimit = 30.0f;

    //[SerializeField] private PlayerManager player;
    [SerializeField] private GameObject crosshair;


    private bool aim;
    private bool aiming;
    private bool isTransitioning;
    private Vector3 transitionPosition;
    private Quaternion transitionRotation;
    [SerializeField] float transitionDuration = 0.5f;
    private float transitionTimer;


    float mouseX, mouseY;

    Vector3 cameraDirection;

    void Start()
    {
        aim = false;
        aiming = false;
        isTransitioning = false;
        crosshair.SetActive(false);
    }

    void Update()
    {
        if (!InputManager.Instance.CameraKeysLocked)
        {
            mouseX += InputManager.Instance.MouseMovementX * rotationSpeed;
            mouseY -= InputManager.Instance.MouseMovementY * rotationSpeed;


            Vector3 cameraForward = transform.forward;
            Vector3 playerGOForward = playerGO.transform.forward;



            playerGOForward.y = 0;

            //float angleBetween = Vector3.Angle(transform.forward, playerGO.forward);


            if (!aiming)
            {
                crosshair.SetActive(false);
                cameraForward.y = 0;
                //if (angleBetween <= 80)
                //{

                //    Vector3 lookBack = target.position + cameraForward * distance;
                //    headLookAt.position = Vector3.Lerp(headLookAt.position,
                //        new Vector3(lookBack.x, target.position.y, lookBack.z), Time.deltaTime * 5f);
                //}
                //else if (angleBetween > 135)
                //{
                //    Vector3 lookBack = target.position + playerGOForward * distance;
                //    headLookAt.position = Vector3.Lerp(headLookAt.position,
                //        new Vector3(lookBack.x, target.position.y, lookBack.z), Time.deltaTime * 5f);
                //}

            }
            else if (aiming)
            {

                crosshair.SetActive(true);
                //headLookAt.position = transform.position + cameraForward * distance;
            }
        }
       


        


    }

    void LateUpdate()
    {
        if (!InputManager.Instance.CameraKeysLocked)
        {
            if (isTransitioning)
            {
                PerformTransition();
            }
            else
            {
                if (!aim)
                {
                    freeFlowCamera();
                }
                else
                {
                    overShoulderCamera();
                }
            }
        }
        
    }

    public void ToggleADS()
    {
        PlayerManager.Instance.ADS = !PlayerManager.Instance.ADS;
        if (!PlayerManager.Instance.ADS)
        {
            ADSDown();
        }
        else
        {
            ADSUp();
        }
    }
    void ADSUp()
    {
        aim = true;
        StartTransitionToOverShoulder();

    }
    void ADSDown()
    {
        aim = false;
        StartTransitionToFreeFlow();
    }
    void StartTransitionToOverShoulder()
    {
        isTransitioning = true;
        transitionTimer = 0f;
        transitionPosition = transform.position;
        transitionRotation = transform.rotation;
    }

    void StartTransitionToFreeFlow()
    {
        isTransitioning = true;
        transitionTimer = 0f;
        transitionPosition = transform.position;
        transitionRotation = transform.rotation;
    }

    void PerformTransition()
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        if (aim)
        {
            transform.position = Vector3.Lerp(transitionPosition, overShoulderCamTarget.position, t);
            transform.rotation = Quaternion.Slerp(transitionRotation, overShoulderCamTarget.rotation, t);
        }
        else
        {
            transform.position = Vector3.Lerp(transitionPosition, target.position - transform.forward * distance, t);
            transform.rotation = Quaternion.Slerp(transitionRotation, Quaternion.Euler(mouseY, mouseX, 0), t);
        }

        if (transitionTimer >= transitionDuration)
        {
            isTransitioning = false;
        }
    }

    void freeFlowCamera()
    {
        aiming = false;
        mouseY = Mathf.Clamp(mouseY, -cameraAngleDownLimit, cameraAngleUpLimit);
        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // Calculate the desired camera position based on no obstruction
        Vector3 desiredCameraPos = target.position - transform.forward * distance;

        RaycastHit hit;
        if (Physics.SphereCast(target.position, 0.5f, -transform.forward, out hit, distance, ~0, QueryTriggerInteraction.Ignore))
        {

            cameraDirection = transform.forward * (hit.distance + minDistance);
            transform.position = target.position - cameraDirection;
        }
        else
        {
            // If no obstruction, set the camera to the desired position
            transform.position = desiredCameraPos;
        }

        // Always look at the target considering the height offset
        transform.LookAt(target.position + new Vector3(0, heightOffset, 0));


    }

    void overShoulderCamera()
    {
        aiming = true;
        mouseY = Mathf.Clamp(mouseY, -aimCameraAngleDownLimit, aimCameraAngleUpLimit);
        playerGO.rotation = Quaternion.Euler(0, mouseX, 0);
        overShoulderCamTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = overShoulderCamTarget.rotation;
        transform.position = overShoulderCamTarget.position;
        transform.LookAt(this.transform);
    }

    public void cameraZoomInOut()
    {
        if (InputManager.Instance.mouseScrollWheel < 0f)
        {
            distance += 0.1f;
        }
        else if (InputManager.Instance.mouseScrollWheel > 0f)
        {
            distance -= 0.1f;
        }
        distance = Mathf.Clamp(distance, 1, 5);
    }

    //private void lerpGunRig(float lerpValue, float lerpDuration)
    //{
    //    transitionTimer += Time.deltaTime;
    //    float t = Mathf.Clamp01(transitionTimer / lerpDuration);
    //    gunRig.weight = Mathf.Lerp(gunRig.weight, lerpValue, t);
    //    if (transitionTimer >= lerpDuration)
    //    {
    //        return;
    //    }
    //}

    //private void lerpADSGunRig(float lerpValue, float lerpDuration)
    //{
    //    transitionTimer += Time.deltaTime;
    //    float t = Mathf.Clamp01(transitionTimer / lerpDuration);
    //    ADSGunRig.weight = Mathf.Lerp(ADSGunRig.weight, lerpValue, t);
    //    if (transitionTimer >= lerpDuration)
    //    {
    //        return;
    //    }
    //}
}
