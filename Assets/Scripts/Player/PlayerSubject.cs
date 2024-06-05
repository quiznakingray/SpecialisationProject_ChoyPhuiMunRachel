using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubject : MonoBehaviour
{
    public List<IPlayerObserver> Observers = new List<IPlayerObserver>();

    public void NotifyObservers(PlayerActions action)
    {
        foreach (var observers in Observers)
        {
            observers.OnNotify(action);
        }

    }
    public void AddObservers(IPlayerObserver observer)
    {
        Observers.Add(observer);
    }

    public void RemoveObserver(IPlayerObserver observer)
    {
        Observers.Remove(observer);
    }

    private bool doingAction = false;
    public void setDoingAction(bool action)
    {
        doingAction = action;
    }

    public bool getDoingAction()
    {
        return doingAction;
    }
}

