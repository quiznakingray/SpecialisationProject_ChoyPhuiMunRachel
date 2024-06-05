using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerAnimation : MonoBehaviour/*,IPlayerObserver*/
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("isADS", PlayerManager.Instance.ADS);
        if (PlayerManager.Instance.ADS)
        {
            playerAnimator.SetFloat("VelocityX", InputManager.Instance.horizontal);
            playerAnimator.SetFloat("VelocityY", InputManager.Instance.vertical);
        }
        else
        {
            if (InputManager.Instance.horizontal != 0 || InputManager.Instance.vertical != 0)
            {
                playerAnimator.SetBool("isMoving", true);
            }
            else
            {
                playerAnimator.SetBool("isMoving", false);
            }
        }
        
    }

    //public void OnNotify(PlayerActions action)
    //{
    //    switch (action)
    //    {
    //        case PlayerActions.Walk:
    //            break;

    //        case PlayerActions.Idle:
    //            break;
    //    }

    //}

    //private void OnEnable()
    //{
    //    player.AddObservers(this);
    //}

    //private void OnDisable()
    //{
    //    player.RemoveObserver(this);
    //}
}
