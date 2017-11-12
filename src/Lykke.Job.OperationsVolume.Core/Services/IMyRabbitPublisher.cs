using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Job.OperationsVolume.Contract;

namespace Lykke.Job.OperationsVolume.Core.Services
{
    public interface IMyRabbitPublisher : IStartable, IStopable
    {
        Task PublishAsync(MyPublishedMessage message);
    }
}