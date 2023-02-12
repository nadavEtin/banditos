using Assets.Scripts.Events;
using Assets.Scripts.GameLevel;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class PlayerController
    {
        private Vector3Int _curPos;
        private ITilemapManager _tileManager;
        private IUIManager _uiManager;
        private int _stepsCounter;

        public PlayerController(ITilemapManager tileManager, IUIManager uiManager, Vector3Int startingPos)
        {
            EventBus.Subscribe(GameplayEvent.MovementInput, MovementInput);
            _tileManager = tileManager;
            _curPos = startingPos;
            _uiManager = uiManager;
        }

        private void MovementInput(BaseEventParams eventParams)
        {
            Vector3Int posToCheck;
            var dir = ((MovementInputParams)eventParams).MoveDirection;

            //x = tileMap rows, y = tileMap columns
            switch (dir)
            {
                case Direction.Up:
                    posToCheck = new Vector3Int(_curPos.x, _curPos.y + 1, 0);
                    break;
                case Direction.Left:
                    posToCheck = new Vector3Int(_curPos.x - 1, _curPos.y, 0);
                    break;
                case Direction.Down:
                    posToCheck = new Vector3Int(_curPos.x, _curPos.y - 1, 0);
                    break;
                case Direction.Right:
                    posToCheck = new Vector3Int(_curPos.x + 1, _curPos.y, 0);
                    break;
                default:
                    Debug.LogError("incorrect direction");
                    return;
            }

            var destinationTile = _tileManager.GetTileAtPos(posToCheck);
            if (AllowMovement(destinationTile))
            {
                _curPos = destinationTile.pos;
                _tileManager.SetPlayerPos(_curPos);
                _uiManager.UpdateStepsCount(++_stepsCounter);
            }
        }

        //determines if movement is allowed to the destination tile
        private bool AllowMovement(LevelTile tile)
        {
            switch (tile.type)
            {
                case TileType.Base:
                    return true;
                case TileType.Wall:
                    return false;
                case TileType.Exit:
                    ExitReached();
                    return true;
                case TileType.Lava:
                    MovedToLavaTile();
                    return false;
                default:
                    Debug.Log("unrecognized tile type");
                    return false;
            }
        }

        private void ExitReached()
        {
            EventBus.Publish(GameplayEvent.GameOver, new GameOverParams(true));
        }

        private void MovedToLavaTile()
        {
            EventBus.Publish(GameplayEvent.GameOver, new GameOverParams(false));
        }
    }
}
