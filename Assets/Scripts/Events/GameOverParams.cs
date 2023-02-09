namespace Assets.Scripts.Events
{
    public class GameOverParams : BaseEventParams
    {
        public bool PlayerWon => _playerWon;
        private readonly bool _playerWon;

        public GameOverParams(bool playerWon)
        {
            _playerWon = playerWon;
        }
    }
}
