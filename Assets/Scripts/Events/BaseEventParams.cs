namespace Assets.Scripts.Events
{
    public abstract class BaseEventParams
    {
        private static readonly EmptyParams _empty = new EmptyParams();

        //For simple events with no parameters
        public static EmptyParams Empty => _empty;
    }
}
