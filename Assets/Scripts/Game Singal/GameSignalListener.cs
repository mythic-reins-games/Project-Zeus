using UnityEngine;
using UnityEngine.Events;

public class GameSignalListener : MonoBehaviour, IGameSignalListener
{

    public GameSignal gameSignal;
    public UnityEvent gameSignalEvent;

    private void OnEnable()
    {
        gameSignal.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameSignal.DeRegisterListener(this);
    }

    public void OnGameSignalRaised()
    {
        gameSignalEvent.Invoke();
    }
}
