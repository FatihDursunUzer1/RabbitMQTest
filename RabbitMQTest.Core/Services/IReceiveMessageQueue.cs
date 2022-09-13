using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Core.Services
{
    public interface IReceiveMessageQueue
    {
        void GetMessageDefaultExchange(string queue);
    }
}
