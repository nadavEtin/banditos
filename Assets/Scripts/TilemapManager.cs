using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct LevelTile
{
    public Vector3Int pos;
    public TileType type;
    public TileType layer2Tile;
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

    public void SetPlayerPos(Vector3Int newPos)
    {
        var oldPos = _player.pos;
        _player.pos = newPos;
        _secondLayer.SetTile(oldPos, null);
        _secondLayer.SetTile(newPos, _tileRefs.TileObjectDictionary[TileType.Player]);
    }

    public LevelTile GetTileAtPos(Vector3Int newPos)
    {
        return _tilesData[newPos.y][newPos.x];
    }

    public void PlayerDied()
    {
        _secondLayer.SetTile(_player.pos, null);
    }

    private void BoardSetup()
    {
        //start creating tiles from the bottom left position, fill rows first
        for (int i = _gameParams.LevelMinYVal; i <= _gameParams.LevelMaxYVal; i++)
        {
            var newRow = new List<LevelTile>();
            _tilesData.Add(newRow);
            for (int j = _gameParams.LevelMinXVal; j <= _gameParams.LevelMaxXVal; j++)
            {
                var newTile = new LevelTile();
                newTile.pos = new Vector3Int(j, i, 0);
                //set border tiles as walls
                if (i == _gameParams.LevelMinYVal || i == _gameParams.LevelMaxXVal ||
                    j == _gameParams.LevelMinXVal || j == _gameParams.LevelMaxXVal)
                    SetTile(ref newTile, TileType.Wall);
                else
                    SetTile(ref newTile, TileType.Base);
                newRow.Add(newTile);
            }
        }
        var exitTile = _tilesData[_gameParams.ExitPos.y][_gameParams.ExitPos.x];
        SetTile(ref exitTile, TileType.Exit);
        _tilesData[exitTile.pos.y][exitTile.pos.x] = exitTile;
        //_firstLayer.SetTile(_gameParams.ExitPos, _tileRefs.TileObjectDictionary[TileType.Exit]);
        SetPlayerPos(_gameParams.PlayerStartPos);
    }

    private void SetTile(ref LevelTile tile, TileType type)
    {
        tile.type = type;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[type]);
    }

    /*private void SetBaseTile(ref LevelTile tile)
    {
        tile.type = TileType.Base;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[TileType.Base]);
    }

    private void SetWallTile(ref LevelTile tile)
    {
        tile.type = TileType.Wall;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[TileType.Wall]);
    }

    private void SetExitTile(ref LevelTile tile)
    {

        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[TileType.Exit]);
    }*/
}
