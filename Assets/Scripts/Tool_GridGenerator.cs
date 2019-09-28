using UnityEditor;
using UnityEngine;

public class Tool_GridGenerator : MonoBehaviour
{
    private static int gridWidth = 12;
    private static string tilePrefabFilePath = "Assets/Prefabs/Tiles/Tile.prefab";

    [MenuItem("Tools/Generate Grid")]
    public static void generateGrid()
    {
        GameObject grid = new GameObject("Grid");
        GameObject tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabFilePath);

        for (int i = 0; i < gridWidth; i++)
        {
            GameObject row = new GameObject("Row" + i);
            row.transform.parent = grid.transform;

            for (int j = 0; j < 12; j++)
            {
                Vector3 position = new Vector3(i, 0, j);
                Quaternion rotation = new Quaternion();
                GameObject tile = Instantiate(tilePrefab, position, rotation);
                tile.transform.parent = row.transform;
                tile.name = "Tile";
            }
        }

        grid.transform.position = new Vector3(-5.5f, 0, -5.5f);
    }
}
