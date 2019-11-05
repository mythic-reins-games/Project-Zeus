#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSignalOneObject : ScriptableObject
{

    [SerializeField] private List<IGameSignalOneObjectListener> listeners = new List<IGameSignalOneObjectListener>();

    public void Raise(object value)
    {
        foreach (var listener in listeners)
        {
            listener.OnGameSignalRaised(value);
        }
    }

    public void RegisterListener(IGameSignalOneObjectListener listener)
    {
        listeners.Add(listener);
    }

    public void DeRegisterListener(IGameSignalOneObjectListener listener)
    {
        listeners.Remove(listener);
    }
}
