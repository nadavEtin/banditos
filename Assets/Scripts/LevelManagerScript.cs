using Assets.Scripts;
using Assets.Scripts.Events;
using Assets.Scripts.Gameplay;
using Assets.Scripts.UI;
using UnityEngine;

[RequireComponent(typeof(IUIManager))]
public class LevelManagerScript : MonoBehaviour
{

    [SerializeField] private TileRefrences _tileRefs;
    [SerializeField] private GameParams _gameParams;

    private ITilemapManager _tileMap;
    private PlayerController _playerMovement;
    private IUIManager _uiManager;
    private InputManager _inputManager;

    private void Awake()
    {
        EventBus.Init();
    }

    private void Start()
    {
        EventBus.Subscribe(GameplayEvent.GameOver, GameOver);
        _uiManager = GetComponent<IUIManager>();
        _tileRefs.Init();
        _tileMap = FindObjectOfType<TilemapManager>();
        _tileMap.Init(_tileRefs, _uiManager, _gameParams);
        _playerMovement = new PlayerController(_tileMap, _uiManager, _gameParams.PlayerStartPos);
        _inputManager = gameObject.AddComponent<InputManager>();
    }

    private void GameOver(BaseEventParams eventParams)
    {
        var parameters = (GameOverParams)eventParams;
        var msg = parameters.PlayerWon ? "The player won!" : "The player lost :(";
        Debug.Log(msg);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(GameplayEvent.GameOver, GameOver);
    }
}
