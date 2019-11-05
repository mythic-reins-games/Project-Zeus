#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSignal : ScriptableObject
{

    [SerializeField] private List<IGameSignalListener> listeners = new List<IGameSignalListener>();

    public void Raise()
    {
        foreach (var listener in listeners)
        {
            listener.OnGameSignalRaised();
        }
    }

    public void RegisterListener(IGameSignalListener listener)
    {
        listeners.Add(listener);
    }

    public void DeRegisterListener(IGameSignalListener listener)
    {
        listeners.Remove(listener);
    }
}
