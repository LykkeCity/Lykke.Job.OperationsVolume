using System.Threading.Tasks;

namespace Lykke.Job.OperationsVolume.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}