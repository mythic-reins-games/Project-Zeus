using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Entity
{
    private static PlayerUnit instance;

    public static PlayerUnit Instance { get => instance; }

    private void Awake ()
    {
        instance = this;
    }
}
