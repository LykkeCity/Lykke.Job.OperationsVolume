namespace Lykke.Job.OperationsVolume.Core.Settings.JobSettings
{
        public class OperationsVolumeSettings
        {
            public DbSettings Db { get; set; }
            public RabbitMqSettings Rabbit { get; set; }
        }
}