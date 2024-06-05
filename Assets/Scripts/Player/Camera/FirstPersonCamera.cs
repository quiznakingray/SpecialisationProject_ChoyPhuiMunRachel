using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform firstPersonCameraTarget;
    [SerializeField] Transform playerGO;
    [SerializeField, Range(0,90)]  float cameraAngleDownLimit = 30f;
    [SerializeField, Range(0,90)]  float cameraAngleUpLimit = 30f;
    [SerializeField] float rotationSpeed = 2f;

    float mouseY, mouseX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputManager.Instance.CameraKeysLocked)
        {
            mouseX += InputManager.Instance.MouseMovementX * rotationSpeed;
            mouseY -= InputManager.Instance.MouseMovementY * rotationSpeed;

            transform.position = firstPersonCameraTarget.position;
            mouseY = Mathf.Clamp(mouseY, -cameraAngleDownLimit, cameraAngleUpLimit);
            playerGO.rotation = Quaternion.Euler(0, mouseX, 0);
            firstPersonCameraTarget.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            transform.rotation = firstPersonCameraTarget.rotation;
            transform.position = firstPersonCameraTarget.position;
            transform.LookAt(this.transform);
        }
        
    }
}
