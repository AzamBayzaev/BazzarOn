using MediatR;

namespace BazzarOn.Mediator.Helper.Events;

public interface IEvent : INotification;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent;
