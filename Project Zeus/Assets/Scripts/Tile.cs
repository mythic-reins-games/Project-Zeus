using UnityEngine;

public class Tile : MonoBehaviour
{
    public Entity currentEntity;

    public Material def;
    public Material redish;

    private void Update ()
    {
        if (Vector3.Distance (transform.position, PlayerUnit.Instance.transform.position)
            < PlayerUnit.Instance.GetComponent<PlayerMovement> ().minDistance)
        {
            GetComponent<Renderer> ().material = redish;
        }
        else
        {
            GetComponent<Renderer> ().material = def;
        }
    }

    private void OnMouseDown ()
    {
        GameEvents.TileSelected (this);
    }
}
