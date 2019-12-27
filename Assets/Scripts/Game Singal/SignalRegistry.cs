using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures there can only be one instance of any GameSignal object in existence at any time.
// This prevents bugs where there are different copies of the same kind of GameSignal.
public static class SignalRegistry
{
    private static GameSignalOneObject concentrationSignal = null;

    public static GameSignalOneObject ConcentrationSignal()
    {
        if (concentrationSignal == null)
            concentrationSignal = (GameSignalOneObject)Resources.Load("Game Signals/SetConcentration", typeof(GameSignalOneObject));
        return concentrationSignal;
    }

}
