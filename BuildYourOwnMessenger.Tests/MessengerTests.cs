using BuildYourOwnMessenger.Services;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace BuildYourOwnMessenger.Tests
{
    public class MessengerTests
    {
        private readonly Messenger _Sut = new Messenger();
        public record FakeMessage(string Message);
        public record AnotherFakeMessage(string Message);
        public interface IFakeObserver { void CallMeWithMessage(object message); }

        [Fact]
        public void Send_NotifiesSubscribers_WhenSingleSubscriber()
        {
            var fakeObserver = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _Sut.Subscribe<FakeMessage>(fakeObserver.Object, fakeObserver.Object.CallMeWithMessage);

            _Sut.Send(message);

            fakeObserver.Verify(x => x.CallMeWithMessage(message), Times.Once());
        }

        [Fact]
        public void Send_DoesNotNotifySubscriber_WhenSubscriberUnsubscribes()
        {
            var fakeObserver = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _Sut.Subscribe<FakeMessage>(fakeObserver.Object, fakeObserver.Object.CallMeWithMessage);
            
            _Sut.Unsubscribe<FakeMessage>(fakeObserver.Object);
            _Sut.Send(message);

            fakeObserver.Verify(x => x.CallMeWithMessage(message), Times.Never());
        }

        [Fact]
        public void Send_NotifiesAllSubscribers_WhenMoreThanOneSubscriber()
        {
            var fakeObserver1 = new Mock<IFakeObserver>();
            var fakeObserver2 = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            _Sut.Subscribe<FakeMessage>(fakeObserver1.Object, fakeObserver1.Object.CallMeWithMessage);
            _Sut.Subscribe<FakeMessage>(fakeObserver2.Object, fakeObserver2.Object.CallMeWithMessage);

            _Sut.Send(message);

            fakeObserver2.Verify(x => x.CallMeWithMessage(message), Times.Once());
            fakeObserver1.Verify(x => x.CallMeWithMessage(message), Times.Once());
        }

        [Fact]
        public void Send_NotifiesOnlySubscribersOfSpecifiedType_WhenMoreThanOneSubscriberAndTypeAreUsed()
        {
            var fakeObserver1 = new Mock<IFakeObserver>();
            var fakeObserver2 = new Mock<IFakeObserver>();
            var message = new FakeMessage(string.Empty);
            var message2 = new AnotherFakeMessage(string.Empty);
            _Sut.Subscribe<FakeMessage>(fakeObserver1.Object, fakeObserver1.Object.CallMeWithMessage);
            _Sut.Subscribe<AnotherFakeMessage>(fakeObserver2.Object, fakeObserver2.Object.CallMeWithMessage);

            _Sut.Send(message);

            fakeObserver1.Verify(x => x.CallMeWithMessage(message), Times.Once());
            fakeObserver2.Verify(x => x.CallMeWithMessage(It.IsAny<object>()), Times.Never());
        }

        [Fact]
        public void Send_ThrowsException_WhenMessageIsNull()
        {
            _Sut.Invoking(x => x.Send(null))
                .Should().Throw<ArgumentNullException>().WithParameterName("message");
        }

        [Fact]
        public void Send_DoesNotThrow_WhenNoSubscribersForMessageTypeExist()
        {
            var message = new FakeMessage(string.Empty);

            _Sut.Send(message);
        }

        [Fact]
        public void Unsubscribe_DoesNotThrow_WhenNotSubscriptionsForTheMessageTypeExist()
        {
            var fakeObserver1 = new Mock<IFakeObserver>();

            _Sut.Unsubscribe<FakeMessage>(fakeObserver1.Object);
        }

        [Fact]
        public void Subscribe_CausesActionToBeCalled_WhenSendHappensBeforeSubscription()
        {
            var message = new FakeMessage(string.Empty);
            _Sut.Send(message);
            var fakeObserver = new Mock<IFakeObserver>();

            _Sut.Subscribe<FakeMessage>(fakeObserver.Object, fakeObserver.Object.CallMeWithMessage);

            fakeObserver.Verify(x => x.CallMeWithMessage(message), Times.Once());
        }
    }
}