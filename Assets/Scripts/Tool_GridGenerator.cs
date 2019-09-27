using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tool_GridGenerator : MonoBehaviour
{
    private static int gridWidth = 10;
    private static string tilePrefabFilePath = "Assets/Prefabs/Tiles/Tile (0).prefab";

    [MenuItem("Tools/Generate Grid")]
    public static void generateGrid()
    {
        GameObject grid = GameObject.Find("Grid");
        GameObject tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabFilePath);

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Vector3 position = new Vector3(i, 0, j);
                Quaternion rotation = new Quaternion();
                GameObject tile = Instantiate(tilePrefab, position, rotation);
                tile.transform.parent = grid.transform;
                tile.name = "Tile" + (gridWidth * i + j);
            }
        }
    }
}
