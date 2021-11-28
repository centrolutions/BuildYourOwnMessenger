using System;

namespace BuildYourOwnMessenger.Services
{
    public interface IMessenger
    {
        void Send<TMessage>(TMessage message);
        void Subscribe<TMessage>(object subscriber, Action<object> action);
        void Unsubscribe<TMessage>(object subscriber);
    }
}