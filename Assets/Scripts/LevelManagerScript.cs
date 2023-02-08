using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManagerScript : MonoBehaviour
{
    
    [SerializeField] private TileRefrences _tileRefs;
    [SerializeField] private GameParams _gameParams;

    private ITilemapManager _tileMap;

    private void Start()
    {
        _tileMap = FindObjectOfType<TilemapManager>();
        _tileMap.Init(_tileRefs);
        /*_tileMap.SetTile(new Vector3Int(-5,-5, -5), _tileRefs._emptyTile);
        _tileMap.SetTile(new Vector3Int(4, 4, 0), _tileRefs._emptyTile);*/
    }
}
