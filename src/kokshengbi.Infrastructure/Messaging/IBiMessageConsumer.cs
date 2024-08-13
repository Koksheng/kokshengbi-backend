using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kokshengbi.Infrastructure.Messaging
{
    public interface IBiMessageConsumer
    {
        Task ConsumeMessage(string message, ulong deliveryTag);
    }
}
