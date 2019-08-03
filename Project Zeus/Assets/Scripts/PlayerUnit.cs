using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Entity
{
    public static PlayerUnit Instance;

    private void Awake ()
    {
        Instance = this;
    }
}
