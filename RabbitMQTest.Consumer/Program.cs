// See https://aka.ms/new-console-template for more information
using RabbitMQTest.Core.Services;

public class Program
{
    private static void Main(string[] args)
    {
       
            Console.WriteLine("Hello, World!");
            RabbitMQService rabbitMQService = new RabbitMQService();

            rabbitMQService.GetMessageDefaultExchange("message_queue");
            Console.ReadKey();
    }
}