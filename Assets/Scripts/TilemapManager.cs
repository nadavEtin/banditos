using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public struct LevelTile
{
    public Vector3Int pos;
    public TileType type;
}

public class TilemapManager : MonoBehaviour, ITilemapManager
{
    [SerializeField] private Tilemap _firstLayer, _secondLayer;

    private List<List<LevelTile>> _tilesData;
    private GameParams _gameParams;
    private TileRefrences _tileRefs;
    private LevelTile _player;

    public void Init(TileRefrences tileRefs, GameParams gameParams)
    {
        _tilesData = new List<List<LevelTile>>();
        _tileRefs = tileRefs;
        _gameParams = gameParams;
        BoardSetup();
    }

    private void BoardSetup()
    {
        for (int i = _gameParams.LevelMinYVal; i <= _gameParams.LevelMaxYVal; i++)
        {
            var newRow = new List<LevelTile>();
            _tilesData.Add(newRow);
            for (int j = _gameParams.LevelMinXVal; j <= _gameParams.LevelMaxXVal; j++)
            {
                var newTile = new LevelTile();
                newRow.Add(newTile);
                newTile.pos = new Vector3Int(j, i, 0);
                //set border tiles as walls
                if (i == _gameParams.LevelMinYVal || i == _gameParams.LevelMaxXVal ||
                    j == _gameParams.LevelMinXVal || j == _gameParams.LevelMaxXVal)
                {
                    SetWallTile(newTile);
                    continue;
                }
                SetBaseTile(newTile);
            }
        }
        _firstLayer.SetTile(_gameParams.ExitPos, _tileRefs.TileObjectDictionary[TileType.Exit]);
        SetPlayerPos(_gameParams.PlayerStartPos);
    }

    private void SetPlayerPos(Vector3Int newPos)
    {
        var oldPos = _player.pos;
        _player.pos = newPos;
        _secondLayer.SetTile(oldPos, null);
        _secondLayer.SetTile(newPos, _tileRefs.TileObjectDictionary[TileType.Player]);
    }

    private void SetBaseTile(LevelTile tile)
    {
        tile.type = TileType.Base;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[TileType.Base]);
    }

    private void SetWallTile(LevelTile tile)
    {
        tile.type = TileType.Wall;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[TileType.Wall]);
    }
}
