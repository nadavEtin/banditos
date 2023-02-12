using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    public class InputManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                EventBus.Publish(GameplayEvent.MovementInput, new MovementInputParams(Direction.Up));
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                EventBus.Publish(GameplayEvent.MovementInput, new MovementInputParams(Direction.Left));
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                EventBus.Publish(GameplayEvent.MovementInput, new MovementInputParams(Direction.Down));
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                EventBus.Publish(GameplayEvent.MovementInput, new MovementInputParams(Direction.Right));
        }
    }
}