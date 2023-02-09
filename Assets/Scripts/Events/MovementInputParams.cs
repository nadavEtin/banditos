namespace Assets.Scripts.Events
{
    public class MovementInputParams : BaseEventParams
    {
        public Direction MoveDirection => _moveDir;
        private readonly Direction _moveDir;

        public MovementInputParams(Direction moveDir)
        {
            _moveDir = moveDir;
        }
    }
}
