using Infra.Services.Interfaces;
using Infra.Services.Models;
using System;
using System.Diagnostics;

namespace Infra.Services.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        public void Handler(QueueMessage message)
        {
            Debug.WriteLine(message.Message);
        }
    }
}
