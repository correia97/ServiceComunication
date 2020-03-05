using Infra.Services.Models;

namespace Infra.Services.Interfaces
{
    public interface IMessageHandler
    {
        void Handler(QueueMessage message);
    }
}
