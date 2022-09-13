using RabbitMQTest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Core.Services
{
   public interface ISendMessageQueue
    {
        void SendMessageDefaultExchange(string message, string queue);
        void SendMessage(string message, string queue, ExchangeTypes exchangeType);
    }
}
