using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class PlayerController : IPlayerController
    {
        private Vector3Int _curPos;
        private ITilemapManager _tileManager;

        public PlayerController(ITilemapManager tileManager, Vector3Int startingPos)
        {
            EventBus.Subscribe(GameplayEvent.GameOver, GameOver);
            EventBus.Subscribe(GameplayEvent.MovementInput, MovementInput);
            _tileManager = tileManager;
            _curPos = startingPos;
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
            }
        }

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
                    LavaTile();
                    return false;
                default:
                    Debug.Log("unrecognized tile type");
                    return false;
            }
        }

        private void ExitReached()
        {
            EventBus.Publish(GameplayEvent.GameOver, new GameOverParams(false));
        }

        private void LavaTile()
        {
            EventBus.Publish(GameplayEvent.GameOver, new GameOverParams(false));
        }

        private void GameOver(BaseEventParams eventParams)
        {
            EventBus.Unsubscribe(GameplayEvent.MovementInput, MovementInput);
            EventBus.Unsubscribe(GameplayEvent.GameOver, GameOver);
        }
    }
}
