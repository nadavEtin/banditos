using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    None,
    Base,
    Wall,
    Exit,
    Lava,
    Player,
    Star
}

[CreateAssetMenu(fileName = "TileReferences", menuName = "ScriptableObjects/Tile references")]
public class TileRefrences : ScriptableObject
{
    public Dictionary<TileType, Tile> TileObjectDictionary;
    [SerializeField] private Tile BaseTile, WallTile, ExitTile, Player;

    public void Init()
    {
        TileObjectDictionary = new Dictionary<TileType, Tile>
        {
            { TileType.Base, BaseTile }, { TileType.Wall, WallTile },
            { TileType.Player, Player }, { TileType.Exit, ExitTile }
        };
    }
}