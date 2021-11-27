using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BuildYourOwnMessenger.Services
{
    public class Messenger : IMessenger
    {
        ConcurrentDictionary<Type, SynchronizedCollection<Subscription>> _Subscriptions = new();
        ConcurrentDictionary<Type, object> _CurrentState = new();

        public void Send(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _CurrentState.TryAdd(message.GetType(), message);

            if (!_Subscriptions.ContainsKey(message.GetType()))
                return;

            foreach (var subscription in _Subscriptions[message.GetType()])
                subscription.Action(message);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object> action)
        {
            var newSubscription = new Subscription(subscriber, action);
            if (!_Subscriptions.ContainsKey(typeof(TMessage)))
                _Subscriptions.TryAdd(typeof(TMessage), new SynchronizedCollection<Subscription>());

            _Subscriptions[typeof(TMessage)].Add(newSubscription);
            if (_CurrentState.ContainsKey(typeof(TMessage)))
                newSubscription.Action(_CurrentState[typeof(TMessage)]);
        }

        public void Unsubscribe<TMessage>(object subscriber)
        {
            if (!_Subscriptions.ContainsKey(typeof(TMessage)))
                return;

            var existingSubscription = _Subscriptions[typeof(TMessage)].FirstOrDefault(x => x.Subscriber == subscriber);
            if (existingSubscription != null)
                _Subscriptions[typeof(TMessage)].Remove(existingSubscription);
        }
    }

    internal record Subscription(object Subscriber, Action<object> Action);
}
