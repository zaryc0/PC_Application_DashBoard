using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<object>> _subscribers = [];

        public EventAggregator() { }

        #region IEventAggregator Members
        /// <summary>
        /// Publish an event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventToPublish"></param>
        public void Publish<TEvent>(TEvent eventToPublish)
        {
            var eventType = typeof(TEvent);
            if (_subscribers.ContainsKey(eventType))
            {
                // Create a copy of the subscribers list to avoid modification issues
                var subscribersCopy = _subscribers[eventType].Cast<ISubscriber<TEvent>>().ToList();

                foreach (var subscriber in subscribersCopy)
                {
                    subscriber.OnEventHandler(eventToPublish);
                }
            }
        }

        /// <summary>
        /// subscribe for to the supplied event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="action"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Subscribe<TEvent>(ISubscriber<TEvent> subscriber)
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<object>();
            }
            _subscribers[eventType].Add(subscriber);
        }

        /// <summary>
        /// unsubscribe to the supplied event type
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="subscriber"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Unsubscribe<TEvent>(ISubscriber<TEvent> subscriber)
        {
            var eventType = typeof(TEvent);
            if ( _subscribers.ContainsKey(eventType))
            {
                if (_subscribers[eventType].Contains(subscriber))
                {
                    _subscribers[eventType].Remove(subscriber);
                }
            }
        }


        #endregion
    }
}
