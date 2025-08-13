using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace SendMessage
{
    public class SendQueueData
    {
        public async Task<bool> PushDataToQueue<T>(Task objectData,string connectionString, string queueName)
        {
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);
            string message = JsonSerializer.Serialize(objectData);
            await sender.SendMessageAsync(new ServiceBusMessage(message));
            return true;
        }
    }
}
