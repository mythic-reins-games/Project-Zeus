using UnityEngine;
using UnityEngine.Events;

public class GameSignalListener : MonoBehaviour, IGameSignalListener
{

    public GameSignal gameSignal;
    public FloatEvent gameSignalEvent;

    private void OnEnable()
    {
        gameSignal.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameSignal.DeRegisterListener(this);
    }

    public void OnGameSignalRaised(object value)
    {
        gameSignalEvent.Invoke(value);
    }
}
