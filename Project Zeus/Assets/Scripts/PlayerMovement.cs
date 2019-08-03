using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float minDistance;

    public Tile startTile;

    private void OnEnable ()
    {
        GameEvents.OnTileSelected += MoveToTile;
    }

    private void OnDisable ()
    {
        GameEvents.OnTileSelected -= MoveToTile;
    }

    private void Start ()
    {
        PlayerData.Instance.currentTile = startTile;
        MoveToTile (startTile);
    }

    private void MoveToTile (Tile tile)
    {
        //temp solution
        if (tile.currentEntity == null)
        {
            if (Vector3.Distance (transform.position, tile.transform.position) < minDistance)
            {
                transform.position = new Vector3 (tile.transform.position.x, 0.5f, tile.transform.position.z);
                PlayerData.Instance.currentTile = tile;
                tile.currentEntity = GetComponent<PlayerUnit> ();

                PlayerData.Instance.currentTile.currentEntity = null;
            }
        }
    }
}
