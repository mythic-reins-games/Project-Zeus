using UnityEngine;

public class PlayerData : MonoBehaviour
{

    private Tile currentTile;

    private static PlayerData instance;

    public static PlayerData Instance { get => instance; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }

    private void Awake ()
    {
        instance = this;
    }
}
