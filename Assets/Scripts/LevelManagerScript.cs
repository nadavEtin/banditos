using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManagerScript : MonoBehaviour
{
    
    [SerializeField] private TileRefrences _tileRefs;
    [SerializeField] private GameParams _gameParams;

    private ITilemapManager _tileMap;
    private InputManager _inputManager;

    private void Start()
    {
        _inputManager = new InputManager();
        _tileRefs.Init();
        _tileMap = FindObjectOfType<TilemapManager>();
        _tileMap.Init(_tileRefs, _gameParams);
        /*_tileMap.SetTile(new Vector3Int(-5,-5, -5), _tileRefs._baseTile);
        _tileMap.SetTile(new Vector3Int(4, 4, 0), _tileRefs._baseTile);*/
    }
}
