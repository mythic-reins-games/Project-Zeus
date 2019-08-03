using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Tile currentTile;

    public static PlayerData Instance;

    private void Awake ()
    {
        Instance = this;
    }
}
