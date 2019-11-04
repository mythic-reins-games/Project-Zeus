#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSignal : ScriptableObject
{

    [SerializeField] private List<IGameSignalListener> listeners = new List<IGameSignalListener>();

    public void Raise(float value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnGameSignalRaised(value);
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
