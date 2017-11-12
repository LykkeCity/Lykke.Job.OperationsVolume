using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Job.OperationsVolume.Core.Services;
using Lykke.Job.OperationsVolume.Core.Settings.JobSettings;
using Lykke.Job.OperationsVolume.Services;
using Lykke.SettingsReader;
using Lykke.Job.OperationsVolume.RabbitSubscribers;
using Lykke.Job.OperationsVolume.Contract;
using Lykke.RabbitMq.Azure;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Job.OperationsVolume.RabbitPublishers;
using AzureStorage.Blob;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Job.OperationsVolume.Modules
{
    public class JobModule : Module
    {
        private readonly OperationsVolumeSettings _settings;
        private readonly IReloadingManager<DbSettings> _dbSettingsManager;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public JobModule(OperationsVolumeSettings settings, IReloadingManager<DbSettings> dbSettingsManager, ILog log)
        {
            _settings = settings;
            _log = log;
            _dbSettingsManager = dbSettingsManager;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            // builder.RegisterType<QuotesPublisher>()
            //  .As<IQuotesPublisher>()
            //  .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
            RegisterRabbitMqSubscribers(builder);
            RegisterRabbitMqPublishers(builder);

            // TODO: Add your dependencies here

            builder.Populate(_services);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            // TODO: You should register each subscriber in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<MyRabbitSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString));
        }

        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            // TODO: You should register each publisher in DI container as publisher specific interface and as IStartable,
            // as singleton and do not autoactivate it

            builder.RegisterType<MyRabbitPublisher>()
                .As<IMyRabbitPublisher>()
                .As<IStartable>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString));
        }
    }
}
