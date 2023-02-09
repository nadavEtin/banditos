using Assets.Scripts;
using Assets.Scripts.Events;
using Assets.Scripts.Gameplay;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

    [SerializeField] private TileRefrences _tileRefs;
    [SerializeField] private GameParams _gameParams;

    private ITilemapManager _tileMap;
    private IPlayerController _playerMovement;
    private InputManager _inputManager;

    private void Start()
    {
        EventBus.Subscribe(GameplayEvent.GameOver, GameOver);
        _tileRefs.Init();
        _tileMap = FindObjectOfType<TilemapManager>();
        _tileMap.Init(_tileRefs, _gameParams);
        _playerMovement = new PlayerController(_tileMap, _gameParams.PlayerStartPos);
        _inputManager = gameObject.AddComponent<InputManager>();        
    }

    private void GameOver(BaseEventParams eventParams)
    {
        Debug.Log("The player won!");
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(GameplayEvent.GameOver, GameOver);
    }
}
