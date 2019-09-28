#pragma warning disable 0649

using UnityEngine;

public class Tile : MonoBehaviour
{
    private Entity currentEntity;

    [SerializeField] private Material def;
    [SerializeField] private Material redish;

    public Entity CurrentEntity { get => currentEntity; set => currentEntity = value; }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerUnit.Instance.transform.position)
            < PlayerUnit.Instance.GetComponent<PlayerMovement>().MinDistance)
        {
            GetComponent<Renderer>().material = redish;
        }
        else
        {
            GetComponent<Renderer>().material = def;
        }
    }

    private void OnMouseDown()
    {
        GameEvents.TileSelected(this);
    }
}
