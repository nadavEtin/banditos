using System;
using System.Collections.Generic;

namespace Events
{
    public enum GameplayEvent
    {
        GameStart, GameEnd
    }

    public static class EventBus
    {
        private static readonly Dictionary<GameplayEvent, List<Action<BaseEventParams>>> _subscription =
            new Dictionary<GameplayEvent, List<Action<BaseEventParams>>>();

        public static void Subscribe(GameplayEvent eventType, Action<BaseEventParams> handler)
        {
            if (_subscription.ContainsKey(eventType) == false)
                _subscription.Add(eventType, new List<Action<BaseEventParams>>());

            if (_subscription[eventType].Contains(handler) == false)
                _subscription[eventType].Add(handler);
        }

        public static void Unsubscribe(GameplayEvent eventType, Action<BaseEventParams> handler)
        {
            if (_subscription.ContainsKey(eventType) == false)
                return;

            var handlersList = _subscription[eventType];
            handlersList.Remove(handler);
        }

        public static void Publish(GameplayEvent eventType, BaseEventParams eventParams)
        {
            if (_subscription.ContainsKey(eventType) == false)
                return;

            var handlers = _subscription[eventType];
            for (int i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                handler?.Invoke(eventParams);
            }
        }
    }
}
