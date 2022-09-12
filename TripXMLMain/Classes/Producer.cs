using Newtonsoft.Json;
//using RabbitMQ.Client;
using System;
using System.Text;

namespace TripXMLMain.Classes
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
        void SendMessage(string message);
    }

    public static class RabbitMQProducer 
    {
        public static void SendMessage<T>(T message)
        {
            var json = JsonConvert.SerializeObject(message);
            SendMessage(json);
        }

        public static void SendMessage(string message)
        {
            //var factory = new ConnectionFactory { HostName = "localhost", UserName = "myuser", Password="mypassword" };
            //using (var connection = factory.CreateConnection())
            //using (var model = connection.CreateModel())
            //{
            //    model.QueueBind("ttc.platform", "ttc.exchange", "green");
            //    var body = Encoding.UTF8.GetBytes(message);
            //    model.BasicPublish(exchange: "ttc.exchange", routingKey: "green", body: body);
            //}
        }
    }
}
