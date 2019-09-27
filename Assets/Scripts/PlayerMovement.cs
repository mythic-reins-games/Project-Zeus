#pragma warning disable 0649

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private Tile startTile;

    public float MinDistance { get => minDistance; set => minDistance = value; }

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
        PlayerData.Instance.CurrentTile = startTile;
        MoveToTile (startTile);
    }

    private void MoveToTile (Tile tile)
    {
        //temp solution
        if (tile.CurrentEntity == null)
        {
            if (Vector3.Distance (transform.position, tile.transform.position) < minDistance)
            {
                transform.position = new Vector3 (tile.transform.position.x, 0.5f, tile.transform.position.z);
                PlayerData.Instance.CurrentTile = tile;
                tile.CurrentEntity = GetComponent<PlayerUnit> ();

                PlayerData.Instance.CurrentTile.CurrentEntity = null;
            }
        }
    }
}
