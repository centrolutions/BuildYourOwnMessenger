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

        public void Send<TMessage>(TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            EnsureSubscriptionDictionaryHasMessageType<TMessage>();
            var messageType = typeof(TMessage);
            _CurrentState.AddOrUpdate(messageType, (o) => message, (o, old) => message);
            foreach (var subscription in _Subscriptions[messageType])
                SendMessageToSubscriber(message, subscription);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object> action)
        {
            EnsureSubscriptionDictionaryHasMessageType<TMessage>();

            var newSubscriber = new Subscription(subscriber, action);
            var messageType = typeof(TMessage);
            _Subscriptions[messageType].Add(newSubscriber);
            if (_CurrentState.ContainsKey(messageType))
                SendMessageToSubscriber(_CurrentState[messageType], newSubscriber);
        }

        public void Unsubscribe<TMessage>(object subscriber)
        {
            var messageType = typeof(TMessage);
            if (!_Subscriptions.ContainsKey(messageType))
                return;

            var subscription = _Subscriptions[messageType].FirstOrDefault(x => x.Subscriber == subscriber);
            if (subscription != null)
                _Subscriptions[messageType].Remove(subscription);
        }



        private void EnsureSubscriptionDictionaryHasMessageType<TMessage>()
        {
            if (!_Subscriptions.ContainsKey(typeof(TMessage)))
                _Subscriptions.TryAdd(typeof(TMessage), new SynchronizedCollection<Subscription>());
        }

        private static void SendMessageToSubscriber<TMessage>(TMessage message, Subscription subscription)
        {
            subscription.Action(message);
        }
    }

    public record Subscription(object Subscriber, Action<object> Action);
}
