using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] Button MenuButton;
    // Start is called before the first frame update
    void Start()
    {
        MenuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMenuButtonClicked()
    {
        GameObject menuUI = Instantiate(UIManager.Instance.menuUIPrefab);
        // find widget parent
        menuUI.transform.parent = UIManager.Instance.GetWidgetManager().transform;
        menuUI.transform.localPosition = Vector3.zero;
    }
}
