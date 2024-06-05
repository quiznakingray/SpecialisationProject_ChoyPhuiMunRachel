using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PauseGame()
    {
        TimeManager.Instance.SetPauseTimer(true);
        InputManager.Instance.CameraKeysLocked = true;
        InputManager.Instance.MovementKeysLocked = true;
    }

    public void UnpauseGame()
    {
        TimeManager.Instance.SetPauseTimer(false);
        InputManager.Instance.CameraKeysLocked = false;
        InputManager.Instance.MovementKeysLocked = false;
    }

}
