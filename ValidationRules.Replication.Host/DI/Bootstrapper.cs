﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

using LinqToDB.Mapping;

using Microsoft.Practices.Unity;

using NuClear.Aggregates.Storage.DI.Unity;
using NuClear.Assembling.TypeProcessing;
using NuClear.ValidationRules.OperationsProcessing;
using NuClear.ValidationRules.OperationsProcessing.Contexts;
using NuClear.ValidationRules.OperationsProcessing.Final;
using NuClear.ValidationRules.OperationsProcessing.Transports;
using NuClear.ValidationRules.Replication.Host.Factories;
using NuClear.ValidationRules.Replication.Host.Factories.Messaging.Processor;
using NuClear.ValidationRules.Replication.Host.Factories.Messaging.Receiver;
using NuClear.ValidationRules.Replication.Host.Factories.Messaging.Transformer;
using NuClear.ValidationRules.Replication.Host.Factories.Replication;
using NuClear.ValidationRules.Replication.Host.Settings;
using NuClear.ValidationRules.Storage;
using NuClear.ValidationRules.Storage.Identitites.Connections;
using NuClear.DI.Unity.Config;
using NuClear.DI.Unity.Config.RegistrationResolvers;
using NuClear.IdentityService.Client.Interaction;
using NuClear.Jobs.Schedulers;
using NuClear.Jobs.Unity;
using NuClear.Messaging.API.Processing.Actors.Accumulators;
using NuClear.Messaging.API.Processing.Actors.Handlers;
using NuClear.Messaging.API.Processing.Actors.Transformers;
using NuClear.Messaging.API.Processing.Actors.Validators;
using NuClear.Messaging.API.Processing.Processors;
using NuClear.Messaging.API.Processing.Stages;
using NuClear.Messaging.API.Receivers;
using NuClear.Messaging.DI.Factories.Unity.Accumulators;
using NuClear.Messaging.DI.Factories.Unity.Common;
using NuClear.Messaging.DI.Factories.Unity.Handlers;
using NuClear.Messaging.DI.Factories.Unity.Processors;
using NuClear.Messaging.DI.Factories.Unity.Processors.Resolvers;
using NuClear.Messaging.DI.Factories.Unity.Receivers;
using NuClear.Messaging.DI.Factories.Unity.Receivers.Resolvers;
using NuClear.Messaging.DI.Factories.Unity.Stages;
using NuClear.Messaging.DI.Factories.Unity.Transformers;
using NuClear.Messaging.DI.Factories.Unity.Transformers.Resolvers;
using NuClear.Messaging.DI.Factories.Unity.Validators;
using NuClear.Messaging.Transports.ServiceBus;
using NuClear.Messaging.Transports.ServiceBus.API;
using NuClear.Messaging.Transports.ServiceBus.LockRenewer;
using NuClear.Metamodeling.Domain.Processors.Concrete;
using NuClear.Metamodeling.Processors;
using NuClear.Metamodeling.Provider;
using NuClear.Metamodeling.Provider.Sources;
using NuClear.Metamodeling.Validators;
using NuClear.Model.Common.Operations.Identity;
using NuClear.OperationsLogging;
using NuClear.OperationsLogging.API;
using NuClear.OperationsLogging.Transports.ServiceBus;
using NuClear.OperationsLogging.Transports.ServiceBus.Serialization.ProtoBuf;
using NuClear.OperationsProcessing.Transports.ServiceBus.Primary;
using NuClear.Replication.Core;
using NuClear.Replication.Core.DataObjects;
using NuClear.Replication.Core.Equality;
using NuClear.Replication.Core.Settings;
using NuClear.Replication.OperationsProcessing;
using NuClear.Replication.OperationsProcessing.Metadata;
using NuClear.Replication.OperationsProcessing.Primary;
using NuClear.Replication.OperationsProcessing.Transports;
using NuClear.Replication.OperationsProcessing.Transports.CorporateBus;
using NuClear.Replication.OperationsProcessing.Transports.ServiceBus;
using NuClear.Replication.OperationsProcessing.Transports.ServiceBus.Factories;
using NuClear.Replication.OperationsProcessing.Transports.SQLStore;
using NuClear.Security;
using NuClear.Security.API;
using NuClear.Security.API.UserContext;
using NuClear.Security.API.UserContext.Identity;
using NuClear.Settings.API;
using NuClear.Settings.Unity;
using NuClear.Storage.API.ConnectionStrings;
using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Writings;
using NuClear.Storage.Core;
using NuClear.Storage.LinqToDB;
using NuClear.Storage.LinqToDB.Connections;
using NuClear.Storage.LinqToDB.Writings;
using NuClear.Storage.Readings;
using NuClear.Telemetry;
using NuClear.Tracing.API;
using NuClear.WCF.Client;
using NuClear.WCF.Client.Config;

using Quartz.Spi;

using Schema = NuClear.ValidationRules.Storage.Schema;

using AccountFacts = NuClear.ValidationRules.Storage.Model.AccountRules.Facts;
using AccountAccessors = NuClear.ValidationRules.Replication.AccountRules.Facts;

using PriceFacts = NuClear.ValidationRules.Storage.Model.PriceRules.Facts;
using PriceAccessors = NuClear.ValidationRules.Replication.PriceRules.Facts;

using ConsistencyFacts = NuClear.ValidationRules.Storage.Model.ConsistencyRules.Facts;
using ConsistencyAccessors = NuClear.ValidationRules.Replication.ConsistencyRules.Facts;

using FirmFacts = NuClear.ValidationRules.Storage.Model.FirmRules.Facts;
using FirmAccessors = NuClear.ValidationRules.Replication.FirmRules.Facts;

using AdvertisementFacts = NuClear.ValidationRules.Storage.Model.AdvertisementRules.Facts;
using AdvertisementAccessors = NuClear.ValidationRules.Replication.AdvertisementRules.Facts;

using ProjectFacts = NuClear.ValidationRules.Storage.Model.ProjectRules.Facts;
using ProjectAccessors = NuClear.ValidationRules.Replication.ProjectRules.Facts;

using ThemeFacts = NuClear.ValidationRules.Storage.Model.ThemeRules.Facts;
using ThemeAccessors = NuClear.ValidationRules.Replication.ThemeRules.Facts;

namespace NuClear.ValidationRules.Replication.Host.DI
{
    public static class Bootstrapper
    {
        public static IUnityContainer ConfigureUnity(ISettingsContainer settingsContainer, ITracer tracer, ITracerContextManager tracerContextManager)
        {
            IUnityContainer container = new UnityContainer();
            var massProcessors = new IMassProcessor[]
                                 {
                                     new TaskServiceJobsMassProcessor(container),
                                 };
            var storageSettings = settingsContainer.AsSettings<ISqlStoreSettingsAspect>();

            container.RegisterContexts()
                     .AttachQueryableContainerExtension()
                     .UseParameterResolvers(ParameterResolvers.Defaults)
                     .ConfigureMetadata()
                     .ConfigureSettingsAspects(settingsContainer)
                     .ConfigureTracing(tracer, tracerContextManager)
                     .ConfigureSecurityAspects()
                     .ConfigureQuartz()
                     .ConfigureIdentityInfrastructure()
                     .ConfigureWcf()
                     .ConfigureOperationsProcessing()
                     .ConfigureStorage(storageSettings, EntryPointSpecificLifetimeManagerFactory)
                     .ConfigureReplication(EntryPointSpecificLifetimeManagerFactory);

            ReplicationRoot.Instance.PerformTypesMassProcessing(massProcessors, true, typeof(object));

            return container;
        }

        private static LifetimeManager EntryPointSpecificLifetimeManagerFactory()
        {
            return Lifetime.PerScope;
        }

        private static IUnityContainer ConfigureTracing(this IUnityContainer container, ITracer tracer, ITracerContextManager tracerContextManager)
        {
            return container.RegisterInstance(tracer)
                            .RegisterInstance(tracerContextManager);
        }

        private static IUnityContainer ConfigureMetadata(this IUnityContainer container)
        {
            // provider
            container.RegisterType<IMetadataProvider, MetadataProvider>(Lifetime.Singleton);

            // processors
            container.RegisterOne2ManyTypesPerTypeUniqueness<IMetadataProcessor, TunedReferencesEvaluatorProcessor>(Lifetime.Singleton);
            container.RegisterOne2ManyTypesPerTypeUniqueness<IMetadataProcessor, Feature2PropertiesLinkerProcessor>(Lifetime.Singleton);

            // validators
            container.RegisterType<IMetadataValidatorsSuite, MetadataValidatorsSuite>(Lifetime.Singleton);

            // register matadata sources without massprocessor
            container.RegisterOne2ManyTypesPerTypeUniqueness(typeof(IMetadataSource), typeof(PerformedOperationsMessageFlowsMetadataSource), Lifetime.Singleton);
            container.RegisterOne2ManyTypesPerTypeUniqueness(typeof(IMetadataSource), typeof(OperationRegistryMetadataSource), Lifetime.Singleton);

            return container;
        }

        private static IUnityContainer ConfigureSecurityAspects(this IUnityContainer container)
        {
            return container
                .RegisterType<IUserAuthenticationService, NullUserAuthenticationService>(Lifetime.PerScope)
                .RegisterType<IUserProfileService, NullUserProfileService>(Lifetime.PerScope)
                .RegisterType<IUserContext, UserContext>(Lifetime.PerScope, new InjectionFactory(c => new UserContext(null, null)))
                .RegisterType<IUserLogonAuditor, LoggerContextUserLogonAuditor>(Lifetime.Singleton)
                .RegisterType<IUserIdentityLogonService, UserIdentityLogonService>(Lifetime.PerScope)
                .RegisterType<ISignInService, WindowsIdentitySignInService>(Lifetime.PerScope)
                .RegisterType<IUserImpersonationService, UserImpersonationService>(Lifetime.PerScope);
        }

        private static IUnityContainer ConfigureQuartz(this IUnityContainer container)
        {
            return container
                .RegisterType<IJobFactory, JobFactory>(Lifetime.Singleton, new InjectionConstructor(container.Resolve<UnityJobFactory>(), container.Resolve<ITracer>()))
                .RegisterType<IJobStoreFactory, JobStoreFactory>(Lifetime.Singleton)
                .RegisterType<ISchedulerManager, SchedulerManager>(Lifetime.Singleton);
        }

        private static IUnityContainer ConfigureWcf(this IUnityContainer container)
        {
            return container
                .RegisterType<IServiceClientSettingsProvider, ServiceClientSettingsProvider>(Lifetime.Singleton)
                .RegisterType<IClientProxyFactory, ClientProxyFactory>(Lifetime.Singleton);
        }

        private static IUnityContainer ConfigureIdentityInfrastructure(this IUnityContainer container)
        {
            return container.RegisterType<IIdentityGenerator, IdentityGenerator>(Lifetime.Singleton)
                            .RegisterType<IIdentityServiceClient, IdentityServiceClient>(Lifetime.Singleton);
        }

        private static IUnityContainer ConfigureOperationsProcessing(this IUnityContainer container)
        {
            container.RegisterType<IOperationIdentityRegistry>(Lifetime.Singleton, new InjectionFactory(x => x.Resolve<OperationIdentityRegistryFactory>().RegistryFor<PriceFactsSubDomain>()))
                    .RegisterType(typeof(IOperationRegistry<>), typeof(EmptyOperationRegistry<>), Lifetime.Singleton)
                    .RegisterType<IEntityTypeExplicitMapping, NoEntityTypeExplicitMapping>(Lifetime.Singleton);

#if DEBUG
            container.RegisterType<ITelemetryPublisher, DebugTelemetryPublisher>(Lifetime.Singleton);
#else
            container.RegisterType<ITelemetryPublisher, CachingTelemetryPublisherDecorator<LogstashTelemetryPublisher>>(Lifetime.Singleton);
#endif

            // primary
            container.RegisterTypeWithDependencies(typeof(CorporateBusOperationsReceiver), Lifetime.PerScope, null)
                     .RegisterTypeWithDependencies(typeof(ServiceBusMessageReceiverTelemetryDecorator), Lifetime.PerScope, null)
                     .RegisterOne2ManyTypesPerTypeUniqueness<IRuntimeTypeModelConfigurator, ProtoBufTypeModelForTrackedUseCaseConfigurator<ErmSubDomain>>(Lifetime.Singleton)
                     .RegisterTypeWithDependencies(typeof(BinaryEntireBrokeredMessage2TrackedUseCaseTransformer), Lifetime.Singleton, null)
                     .RegisterType<IXmlEventSerializer, XmlEventSerializer>();

            // final
            container.RegisterTypeWithDependencies(typeof(AggregateCommandsHandler), Lifetime.PerResolve, null);

            container.RegisterType<IEventLoggingStrategyProvider, UnityEventLoggingStrategyProvider>()
                     .RegisterType<IEvent2BrokeredMessageConverter<IEvent>, Event2BrokeredMessageConverter>()
                     .RegisterType<IEventLogger, SequentialEventLogger>()
                     .RegisterType<IServiceBusMessageSender, ServiceBusMessageSender>();

            return container.RegisterInstance<IParentContainerUsedRegistrationsContainer>(new ParentContainerUsedRegistrationsContainer(typeof(IUserContext)), Lifetime.Singleton)
                            .RegisterType(typeof(ServiceBusMessageFlowReceiver), Lifetime.Singleton)
                            .RegisterType<IServiceBusLockRenewer, NullServiceBusLockRenewer>(Lifetime.Singleton)
                            .RegisterType<IServiceBusSettingsFactory, ServiceBusSettingsFactory>(Lifetime.Singleton)
                            .RegisterType<IServiceBusMessageFlowReceiverFactory, ServiceBusMessageFlowReceiverFactory>(Lifetime.PerScope)
                            .RegisterType<IMessageProcessingStagesFactory, UnityMessageProcessingStagesFactory>(Lifetime.PerScope)
                            .RegisterType<IMessageFlowProcessorFactory, UnityMessageFlowProcessorFactory>(Lifetime.PerScope)
                            .RegisterType<IMessageReceiverFactory, UnityMessageReceiverFactory>(Lifetime.PerScope)

                            .RegisterOne2ManyTypesPerTypeUniqueness<IMessageFlowProcessorResolveStrategy, PrimaryProcessorResolveStrategy>(Lifetime.Singleton)
                            .RegisterOne2ManyTypesPerTypeUniqueness<IMessageFlowProcessorResolveStrategy, FinalProcessorResolveStrategy>(Lifetime.PerScope)

                            .RegisterOne2ManyTypesPerTypeUniqueness<IMessageFlowReceiverResolveStrategy, MessageFlowReceiverResolveStrategy>(Lifetime.PerScope)

                            .RegisterType<IMessageValidatorFactory, UnityMessageValidatorFactory>(Lifetime.PerScope)
                            .RegisterType<IMessageTransformerFactory, UnityMessageTransformerFactory>(Lifetime.PerScope)

                            .RegisterOne2ManyTypesPerTypeUniqueness<IMessageTransformerResolveStrategy, PrimaryMessageTransformerResolveStrategy>(Lifetime.PerScope)
                            .RegisterType<IMessageProcessingHandlerFactory, UnityMessageProcessingHandlerFactory>(Lifetime.PerScope)
                            .RegisterType<IMessageProcessingContextAccumulatorFactory, UnityMessageProcessingContextAccumulatorFactory>(Lifetime.PerScope);
        }

        private static IUnityContainer ConfigureStorage(this IUnityContainer container, ISqlStoreSettingsAspect storageSettings, Func<LifetimeManager> entryPointSpecificLifetimeManagerFactory)
        {
            // разрешаем update на таблицу состоящую только из Primary Keys
            LinqToDB.Common.Configuration.Linq.IgnoreEmptyUpdate = true;

            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.Zero };

            var schemaMapping = new Dictionary<string, MappingSchema>
                                {
                                    { Scope.Erm, Schema.Erm },
                                    { Scope.Facts, Schema.Facts },
                                    { Scope.Aggregates, Schema.Aggregates },
                                    { Scope.Messages, Schema.Messages },
                                };

            return container
                .RegisterType<IPendingChangesHandlingStrategy, NullPendingChangesHandlingStrategy>(Lifetime.Singleton)
                .RegisterType<IStorageMappingDescriptorProvider, StorageMappingDescriptorProvider>(Lifetime.Singleton)
                .RegisterType<IEntityContainerNameResolver, DefaultEntityContainerNameResolver>(Lifetime.Singleton)
                .RegisterType<IManagedConnectionStateScopeFactory, ManagedConnectionStateScopeFactory>(Lifetime.Singleton)
                .RegisterType<IDomainContextScope, DomainContextScope>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<ScopedDomainContextsStore>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IReadableDomainContext, CachingReadableDomainContext>(entryPointSpecificLifetimeManagerFactory())
                .RegisterInstance<ILinqToDbModelFactory>(
                    new LinqToDbModelFactory(schemaMapping, transactionOptions, storageSettings.SqlCommandTimeout), Lifetime.Singleton)
                .RegisterInstance<IObjectPropertyProvider>(
                    new LinqToDbPropertyProvider(schemaMapping.Values.ToArray()), Lifetime.Singleton)
                .RegisterType<IWritingStrategyFactory, WritingStrategyFactory>()
                .RegisterType<IReadableDomainContextFactory, LinqToDBDomainContextFactory>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IModifiableDomainContextFactory, LinqToDBDomainContextFactory>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IQuery, Query>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType(typeof(IRepository<>), typeof(LinqToDBRepository<>), entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IReadableDomainContextProvider, ReadableDomainContextProvider>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IModifiableDomainContextProvider, ModifiableDomainContextProvider>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType(typeof(IBulkRepository<>), typeof(BulkRepository<>), entryPointSpecificLifetimeManagerFactory())
                .ConfigureReadWriteModels();
        }

        private static IUnityContainer ConfigureReplication(this IUnityContainer container, Func<LifetimeManager> entryPointSpecificLifetimeManagerFactory)
        {
            return container
                .RegisterType<IDataObjectTypesProvider, DataObjectTypesProvider>(Lifetime.Singleton)
                .RegisterType<IEqualityComparerFactory, EqualityComparerFactory>(Lifetime.Singleton)

                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::AssociatedPosition>, PriceAccessors::AssociatedPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::AssociatedPosition>, PriceAccessors::AssociatedPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::AssociatedPositionsGroup>, PriceAccessors::AssociatedPositionsGroupAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::AssociatedPositionsGroup>, PriceAccessors::AssociatedPositionsGroupAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Category>, PriceAccessors::CategoryAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Category>, PriceAccessors::CategoryAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::DeniedPosition>, PriceAccessors::DeniedPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::DeniedPosition>, PriceAccessors::DeniedPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Order>, PriceAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Order>, PriceAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::OrderPosition>, PriceAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::OrderPosition>, PriceAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::OrderPositionAdvertisement>, PriceAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::OrderPositionAdvertisement>, PriceAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Position>, PriceAccessors::PositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Position>, PriceAccessors::PositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Price>, PriceAccessors::PriceAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Price>, PriceAccessors::PriceAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::PricePosition>, PriceAccessors::PricePositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::PricePosition>, PriceAccessors::PricePositionAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::PricePositionNotActive>, PriceAccessors::PricePositionNotActiveAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::PricePositionNotActive>, PriceAccessors::PricePositionNotActiveAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Project>, PriceAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Project>, PriceAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::RulesetRule>, PriceAccessors::RulesetRuleAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::RulesetRule>, PriceAccessors::RulesetRuleAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IStorageBasedDataObjectAccessor<PriceFacts::Theme>, PriceAccessors::ThemeAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<PriceFacts::Theme>, PriceAccessors::ThemeAccessor>(entryPointSpecificLifetimeManagerFactory())

                .RegisterAccessor<AccountFacts::Account, AccountAccessors::AccountAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::Order, AccountAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::Project, AccountAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::Lock, AccountAccessors::LockAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::OrderPosition, AccountAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::ReleaseWithdrawal, AccountAccessors::ReleaseWithdrawalAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AccountFacts::UnlimitedOrder, AccountAccessors::UnlimitedOrderAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterAccessor<ConsistencyFacts::Bargain, ConsistencyAccessors::BargainAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::BargainScanFile, ConsistencyAccessors::BargainScanFileAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Bill, ConsistencyAccessors::BillAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::BranchOffice, ConsistencyAccessors::BranchOfficeAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::BranchOfficeOrganizationUnit, ConsistencyAccessors::BranchOfficeOrganizationUnitAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Category, ConsistencyAccessors::CategoryAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::CategoryFirmAddress, ConsistencyAccessors::CategoryFirmAddressAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Deal, ConsistencyAccessors::DealAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Firm, ConsistencyAccessors::FirmAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::FirmAddress, ConsistencyAccessors::FirmAddressAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::LegalPerson, ConsistencyAccessors::LegalPersonAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::LegalPersonProfile, ConsistencyAccessors::LegalPersonProfileAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Order, ConsistencyAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::OrderPosition, ConsistencyAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::OrderPositionAdvertisement, ConsistencyAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::OrderScanFile, ConsistencyAccessors::OrderScanFileAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Position, ConsistencyAccessors::PositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::Project, ConsistencyAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ConsistencyFacts::ReleaseWithdrawal, ConsistencyAccessors::ReleaseWithdrawalAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterAccessor<FirmFacts::Firm, FirmAccessors::FirmAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::FirmAddress, FirmAccessors::FirmAddressAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::FirmAddressCategory, FirmAccessors::FirmAddressCategoryAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::Order, FirmAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::OrderPosition, FirmAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::OrderPositionAdvertisement, FirmAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::SpecialPosition, FirmAccessors::SpecialPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<FirmFacts::Project, FirmAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterAccessor<ProjectFacts::Category, ProjectAccessors::CategoryAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::CategoryOrganizationUnit, ProjectAccessors::CategoryOrganizationUnitAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::CostPerClickCategoryRestriction, ProjectAccessors::CostPerClickCategoryRestrictionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::FirmAddress, ProjectAccessors::FirmAddressAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::Order, ProjectAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::OrderPosition, ProjectAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::OrderPositionAdvertisement, ProjectAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::OrderPositionCostPerClick, ProjectAccessors::OrderPositionCostPerClickAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::Position, ProjectAccessors::PositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::PricePosition, ProjectAccessors::PricePositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::Project, ProjectAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::ReleaseInfo, ProjectAccessors::ReleaseInfoAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ProjectFacts::SalesModelCategoryRestriction, ProjectAccessors::SalesModelCategoryRestrictionAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterAccessor<AdvertisementFacts::Order, AdvertisementAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::Project, AdvertisementAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::OrderPosition, AdvertisementAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::OrderPositionAdvertisement, AdvertisementAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::PricePosition, AdvertisementAccessors::PricePositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::Position, AdvertisementAccessors::PositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::AdvertisementTemplate, AdvertisementAccessors::AdvertisementTemplateAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::Advertisement, AdvertisementAccessors::AdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::Firm, AdvertisementAccessors::FirmAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::FirmAddress, AdvertisementAccessors::FirmAddressAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::FirmAddressWebsite, AdvertisementAccessors::FirmAddressWebsiteAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::AdvertisementElement, AdvertisementAccessors::AdvertisementElementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<AdvertisementFacts::AdvertisementElementTemplate, AdvertisementAccessors::AdvertisementElementTemplateAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterAccessor<ThemeFacts::Theme, ThemeAccessors::ThemeAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::ThemeCategory, ThemeAccessors::ThemeCategoryAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::ThemeOrganizationUnit, ThemeAccessors::ThemeOrganizationUnitAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::Category, ThemeAccessors::CategoryAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::Order, ThemeAccessors::OrderAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::OrderPosition, ThemeAccessors::OrderPositionAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::OrderPositionAdvertisement, ThemeAccessors::OrderPositionAdvertisementAccessor>(entryPointSpecificLifetimeManagerFactory)
                .RegisterAccessor<ThemeFacts::Project, ThemeAccessors::ProjectAccessor>(entryPointSpecificLifetimeManagerFactory)

                .RegisterType<IDataObjectsActorFactory, UnityDataObjectsActorFactory>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IAggregateActorFactory, UnityAggregateActorFactory>(entryPointSpecificLifetimeManagerFactory());
        }

        private static IUnityContainer RegisterAccessor<TFact, TAccessor>(this IUnityContainer container, Func<LifetimeManager> entryPointSpecificLifetimeManagerFactory)
            where TAccessor : IStorageBasedDataObjectAccessor<TFact>, IDataChangesHandler<TFact>
            => container
                .RegisterType<IStorageBasedDataObjectAccessor<TFact>, TAccessor>(entryPointSpecificLifetimeManagerFactory())
                .RegisterType<IDataChangesHandler<TFact>, TAccessor>(entryPointSpecificLifetimeManagerFactory());

        private static IUnityContainer ConfigureReadWriteModels(this IUnityContainer container)
        {
            var readConnectionStringNameMap = new Dictionary<string, IConnectionStringIdentity>
                {
                    { Scope.Erm, ErmConnectionStringIdentity.Instance },
                    { Scope.Facts, FactsConnectionStringIdentity.Instance },
                    { Scope.Aggregates, AggregatesConnectionStringIdentity.Instance },
                    { Scope.Messages, MessagesConnectionStringIdentity.Instance },
                };

            var writeConnectionStringNameMap = new Dictionary<string, IConnectionStringIdentity>
                {
                    { Scope.Facts, FactsConnectionStringIdentity.Instance },
                    { Scope.Aggregates, AggregatesConnectionStringIdentity.Instance },
                    { Scope.Messages, MessagesConnectionStringIdentity.Instance },
                };

            return container.RegisterInstance<IConnectionStringIdentityResolver>(new ConnectionStringIdentityResolver(readConnectionStringNameMap, writeConnectionStringNameMap));
        }

        private static IUnityContainer RegisterContexts(this IUnityContainer container)
        {
            return container.RegisterInstance(EntityTypeMap.CreateErmContext())
                            .RegisterInstance(EntityTypeMap.CreateAggregateContext())
                            .RegisterInstance(EntityTypeMap.CreateAccountFactsContext())
                            .RegisterInstance(EntityTypeMap.CreateConsistencyFactsContext())
                            .RegisterInstance(EntityTypeMap.CreatePriceFactsContext())
                            .RegisterInstance(EntityTypeMap.CreateFirmFactsContext())
                            .RegisterInstance(EntityTypeMap.CreateProjectFactsContext())
                            .RegisterInstance(EntityTypeMap.CreateAdvertisementFactsContext())
                            .RegisterInstance(EntityTypeMap.CreateThemeFactsContext());
        }

        private static class Scope
        {
            public const string Erm = "Erm";
            public const string Facts = "Facts";
            public const string Aggregates = "Aggregates";
            public const string Messages = "Messages";
        }
    }
}