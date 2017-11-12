using Lykke.Job.OperationsVolume.Core.Settings.JobSettings;
using Lykke.Job.OperationsVolume.Core.Settings.SlackNotifications;

namespace Lykke.Job.OperationsVolume.Core.Settings
{
    public class AppSettings
    {
        public OperationsVolumeSettings OperationsVolumeJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}