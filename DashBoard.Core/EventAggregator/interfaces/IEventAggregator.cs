using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.interfaces
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent eventToPublish);

        void Subscribe<TEvent>(ISubscriber<TEvent> subscriber);

        void Unsubscribe<TEvent>(ISubscriber<TEvent> subscriber);
    }
}
