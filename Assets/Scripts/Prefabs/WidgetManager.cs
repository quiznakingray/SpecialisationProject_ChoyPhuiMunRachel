using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetManager : MonoBehaviour
{
    [SerializeField] GameObject BlackBackgroundGO;
    // Start is called before the first frame update
    void Start()
    {
        BlackBackgroundGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentsInChildren<WidgetBase>().Length > 0)
        {
            BlackBackgroundGO.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else
        {
            BlackBackgroundGO.SetActive(false);
            GameManager.Instance.UnpauseGame();
        }
    }


}
