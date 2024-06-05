using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : WidgetBase
{
    [SerializeField] public Button CancelButton;
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.SetPauseTimer(true);
        //CancelButton.onClick.AddListener(CancelButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryButtonClicked()
    {
        GameObject inventoryPrefab = Instantiate(UIManager.Instance.inventoryPrefab);
        inventoryPrefab.transform.parent = UIManager.Instance.GetWidgetManager().transform;
        inventoryPrefab.transform.localPosition = Vector3.zero;
        //todo: set sibling as the first one( like at the top  of the screen)
        TimeManager.Instance.SetPauseTimer(false);
    }
}
