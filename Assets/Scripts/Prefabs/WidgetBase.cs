using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.SetPauseTimer(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CancelButtonClicked()
    {
        UIManager.Instance.DestroyCurrentWidget(this.gameObject);
    }
   
}
