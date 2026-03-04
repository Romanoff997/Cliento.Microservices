using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliento.Microservices.Shared.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(string routingKey, object @event);
    }
}
