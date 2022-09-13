using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQTest.Core.Enums;
using RabbitMQTest.Core.Services;

namespace RabbitMQTest.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQTestController : ControllerBase
    {
        private readonly IMessageQueue _messageQueue;

        public RabbitMQTestController()
        {
            _messageQueue = new RabbitMQService();
        }

        [HttpPost]
        public void SendMessageToRabbitMQ([FromBody] string message)
        {
            _messageQueue.SendMessageDefaultExchange(message, "message_queue");
        } 

        [HttpPost ("SendMessageWithExchange")]
        public bool SendMessageToRabbitMQWithExchangeType([FromBody] string messageToRabbitMq)
        {
            //_messageQueue.SendMessage(messageToRabbitMq, "test_queue",ExchangeTypes.direct);
            _messageQueue.SendMessage(messageToRabbitMq, "my_test", ExchangeTypes.fanout);
            return true;
        }
    }
}
