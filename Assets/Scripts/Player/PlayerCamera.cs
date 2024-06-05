using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour , IPlayerObserver
{
    [SerializeField] GameObject FirstPersonCamera;
    [SerializeField] GameObject ThirdPersonCamera;

    [SerializeField] private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        FirstPersonCamera.SetActive(false);
        ThirdPersonCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FirstPersonCamera.SetActive(player.isFirstPersonView);
        ThirdPersonCamera.SetActive(!player.isFirstPersonView);

    }

    public void OnNotify(PlayerActions action)
    {
        switch (action)
        {
            case PlayerActions.ToggleFOV:
                if (!player.ADS)
                {
                    player.isFirstPersonView = !player.isFirstPersonView;
                }
                break;

            case PlayerActions.ToggleADS:
                if (!player.isFirstPersonView)
                {
                    ThirdPersonCamera.GetComponent<ThirdPersonCamera>().ToggleADS();
                }
                break;

            case PlayerActions.CameraZoomInOut:
                if (!player.isFirstPersonView)
                {
                    ThirdPersonCamera.GetComponent<ThirdPersonCamera>().cameraZoomInOut();
                }
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
