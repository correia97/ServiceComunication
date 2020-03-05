namespace Infra.Service.Interfaces
{
    public interface IQueueService
    {
        void PublishMessage<T>(T message) where T : class;
        T GetMessage<T>() where T : class;
    }
}
