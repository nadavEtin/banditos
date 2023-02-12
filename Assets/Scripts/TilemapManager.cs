using Assets.Scripts;
using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelTile
{
    public Vector3Int pos;
    public TileType type;
    public TileType layer2Tile;

    public LevelTile() { }
    public LevelTile(Vector3Int pos, TileType type, TileType layer2Tile)
    {
        this.pos = pos;
        this.type = type;
        this.layer2Tile = layer2Tile;
    }
}

public class TilemapManager : MonoBehaviour, ITilemapManager
{
    [SerializeField] private Tilemap _firstLayer, _secondLayer;

    private List<List<LevelTile>> _tilesData;
    private GameParams _gameParams;
    private TileRefrences _tileRefs;
    private IUIManager _uiManager;
    private LevelTile _player;
    private int _starsCollected;

    public void Init(TileRefrences tileRefs, IUIManager uiManager, GameParams gameParams)
    {
        _tilesData = new List<List<LevelTile>>();
        _player = new LevelTile();
        _tileRefs = tileRefs;
        _uiManager = uiManager;
        _gameParams = gameParams;
        BoardSetup();
    }

    public void SetPlayerPos(Vector3Int newPos)
    {
        var oldPos = _player.pos;
        _player.pos = newPos;
        _player.layer2Tile = TileType.Player;
        _tilesData[newPos.x][newPos.y].layer2Tile = TileType.Player;
        _secondLayer.SetTile(oldPos, null);
        _secondLayer.SetTile(newPos, _tileRefs.TileObjectDictionary[TileType.Player]);
        CheckForCollectible();
    }

    public LevelTile GetTileAtPos(Vector3Int tilePos)
    {
        return _tilesData[tilePos.x][tilePos.y];
    }

    public void PlayerDied()
    {
        _secondLayer.SetTile(_player.pos, null);
    }

    private void CheckForCollectible()
    {
        var playerPosTile = _tilesData[_player.pos.x][_player.pos.y];
        if (playerPosTile.layer2Tile == TileType.Star)
            _uiManager.UpdateStarsCount(++_starsCollected);        
    }

    private void BoardSetup()
    {
        //start creating tiles from the bottom left position, fill rows first
        for (int i = 0; i < _gameParams.RowAmnt; i++)
        {
            var newRow = new List<LevelTile>();
            _tilesData.Add(newRow);
            for (int j = 0; j < _gameParams.ColumnAmnt; j++)
            {
                var newTile = new LevelTile();
                newTile.pos = new Vector3Int(i, j, 0);
                //set border tiles as walls
                if (i == 0 || i == _gameParams.RowAmnt - 1 ||
                    j == 0 || j == _gameParams.ColumnAmnt - 1)
                    SetTile(newTile, TileType.Wall);
                else
                    SetTile(newTile, TileType.Base);
                newRow.Add(newTile);
            }
        }
        var exitTile = _tilesData[_gameParams.ExitPos.y][_gameParams.ExitPos.x];
        _tilesData[exitTile.pos.y][exitTile.pos.x] = SetTile(exitTile, TileType.Exit);
        SetPlayerPos(_gameParams.PlayerStartPos);
        CreateObstacles(_player.pos, exitTile.pos);
        AddStars();
    }

    private void AddStars()
    {
        for (int i = 0; i < _gameParams.StarTiles; i++)
        {
            var row = Random.Range(0, _tilesData.Count);
            var col = Random.Range(0, _tilesData[row].Count);
            if (_tilesData[row][col].type == TileType.Base && _tilesData[row][col].layer2Tile == TileType.None)
            {
                _tilesData[row][col].layer2Tile = TileType.Star;
                _secondLayer.SetTile(_tilesData[row][col].pos, _tileRefs.TileObjectDictionary[TileType.Star]);
            }
            else
                i--;
        }
    }

    private void CreateObstacles(Vector3Int playerPos, Vector3Int exitPos)
    {
        //creating a deep clone for placing obstacles
        var tiles = new List<List<LevelTile>>();
        for (int i = 0; i < _tilesData.Count; i++)
        {
            tiles.Add(_tilesData[i].ConvertAll(tile => new LevelTile(tile.pos, tile.type, tile.layer2Tile)));
        }

        //remove occupied tiles from the list
        tiles[playerPos.y][playerPos.x].type = TileType.None;
        tiles[exitPos.y][exitPos.x].type = TileType.None;

        for (int i = 0; i < _gameParams.LavaTiles; i++)
        {
            if (AssignTile(tiles, TileType.Lava) == false)
                i--;
        }
        for (int i = 0; i < _gameParams.WallTiles; i++)
        {
            if (AssignTile(tiles, TileType.Wall) == false)
                i--;
        }
    }

    private bool AssignTile(List<List<LevelTile>> tiles, TileType tileType)
    {
        var row = Random.Range(0, tiles.Count);
        var col = Random.Range(0, tiles[row].Count);
        if (tiles[row][col].type == TileType.Base)
        {
            _tilesData[row][col] = SetTile(tiles[row][col], tileType);
            if (row == 1)
                RemoveAdjacentTiles(row, col, true, true, tiles);
            else if (row == tiles.Count - 2)
                RemoveAdjacentTiles(row, col, true, false, tiles);
            else if (col == 1)
                RemoveAdjacentTiles(row, col, false, true, tiles);
            else if (col == tiles[row].Count - 2)
                RemoveAdjacentTiles(row, col, false, false, tiles);
            return true;
        }
        else
            return false;
    }

    //prevent surrounding tiles of obstacles on the border from having obstacles to ensure a path to the exit
    private void RemoveAdjacentTiles(int row, int col, bool rowBorderPos, bool bottom, List<List<LevelTile>> tiles)
    {
        if (rowBorderPos)
        {
            var nextRow = bottom ? row + 1 : row - 1;
            if (col > 1)
            {
                tiles[row][col - 1].type = TileType.None;
                tiles[nextRow][col - 1].type = TileType.None;
            }
            tiles[nextRow][col].type = TileType.None;
            if (col < tiles[row].Count - 1)
            {
                tiles[row][col + 1].type = TileType.None;
                tiles[nextRow][col + 1].type = TileType.None;
            }
        }
        else
        {
            var nextCol = bottom ? col + 1 : col - 1;
            tiles[row + 1][col].type = TileType.None;
            tiles[row + 1][nextCol].type = TileType.None;
            tiles[row][nextCol].type = TileType.None;
            tiles[row - 1][nextCol].type = TileType.None;
            tiles[row - 1][col].type = TileType.None;
        }
    }

    private LevelTile SetTile(LevelTile tile, TileType type)
    {
        tile.type = type;
        _firstLayer.SetTile(tile.pos, _tileRefs.TileObjectDictionary[type]);
        return tile;
    }
}
