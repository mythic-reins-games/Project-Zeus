using System;

public static class GameEvents
{
    public static event Action<Tile> OnTileSelected;

    public static void TileSelected (Tile tile)
    {
        if (OnTileSelected != null)
        {
            OnTileSelected (tile);
        }
    }
}
