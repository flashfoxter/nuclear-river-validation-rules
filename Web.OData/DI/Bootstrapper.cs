﻿using System;
using System.Net.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData.Extensions;

using Microsoft.Practices.Unity;

using NuClear.AdvancedSearch.EntityDataModel.EntityFramework.Building;
using NuClear.AdvancedSearch.EntityDataModel.EntityFramework.Emit;
using NuClear.AdvancedSearch.EntityDataModel.Metadata;
using NuClear.AdvancedSearch.Web.OData.DataAccess;
using NuClear.AdvancedSearch.Web.OData.DynamicControllers;
using NuClear.AdvancedSearch.Web.OData.Settings;
using NuClear.DI.Unity.Config;
using NuClear.Metamodeling.Processors;
using NuClear.Metamodeling.Provider;
using NuClear.Metamodeling.Provider.Sources;
using NuClear.Settings.API;
using NuClear.Tracing.API;
using NuClear.Tracing.Environment;
using NuClear.Tracing.Log4Net;
using NuClear.Tracing.Log4Net.Config;

namespace NuClear.AdvancedSearch.Web.OData.DI
{
    internal static class Bootstrapper
    {
        public static IUnityContainer ConfigureUnity(ISettingsContainer settingsContainer)
        {
            var container = new UnityContainer()
                .ConfigureMetadata()
                .ConfigureStoreModel()
                .ConfigureWebApiOData()
                .ConfigureTracer(settingsContainer.AsSettings<ITracerSettings>());

            return container;
        }

        public static IUnityContainer ConfigureTracer(this IUnityContainer container, ITracerSettings settings)
        {
            var tracerContextEntryProviders =
                    new ITracerContextEntryProvider[] 
                    {
                        new TracerContextConstEntryProvider(TracerContextKeys.Required.Environment, settings.EnvironmentName),
                        new TracerContextConstEntryProvider(TracerContextKeys.Required.EntryPoint, settings.EntryPointName),
                        new TracerContextConstEntryProvider(TracerContextKeys.Required.EntryPointHost, NetworkInfo.ComputerFQDN),
                        new TracerContextConstEntryProvider(TracerContextKeys.Required.EntryPointInstanceId, Guid.NewGuid().ToString()),
                        new TracerContextSelfHostedEntryProvider(TracerContextKeys.Required.UserAccount)
                    };

            var tracerContextManager = new TracerContextManager(tracerContextEntryProviders);
            var tracer = Log4NetTracerBuilder.Use
                                             .DefaultXmlConfig
                                             .EventLog
                                             .DB(settings.ConnectionString)
                                             .Build;

            return container.RegisterInstance(tracer)
                            .RegisterInstance(tracerContextManager);
        }

        public static IUnityContainer ConfigureMetadata(this IUnityContainer container)
        {
            var metadataSources = new IMetadataSource[]
            {
                new AdvancedSearchMetadataSource()
            };

            var metadataProcessors = new IMetadataProcessor[] { };

            return container.RegisterType<IMetadataProvider, MetadataProvider>(Lifetime.Singleton, new InjectionConstructor(metadataSources, metadataProcessors));
        }

        public static IUnityContainer ConfigureStoreModel(this IUnityContainer container)
        {
            return container
                .RegisterType<ITypeProvider, EmitTypeProvider>(Lifetime.Singleton)
                .RegisterType<EdmxModelBuilder>(Lifetime.Singleton, new InjectionConstructor(typeof(IMetadataProvider), typeof(ITypeProvider)))
                .RegisterType<ODataConnectionFactory>(Lifetime.Singleton);
        }

        public static IUnityContainer ConfigureWebApiOData(this IUnityContainer container)
        {
            return container
                .RegisterType<IDynamicAssembliesRegistry, DynamicAssembliesRegistry>(Lifetime.Singleton)
                .RegisterType<IDynamicAssembliesResolver, DynamicAssembliesRegistry>(Lifetime.Singleton)

                // custom IHttpControllerTypeResolver
                .RegisterType<IHttpControllerTypeResolver, DynamicControllerTypeResolver>(Lifetime.Singleton);
        }

        public static IUnityContainer ConfigureHttpRequest(this IUnityContainer container, HttpRequestMessage request)
        {
            var edmModel = request.ODataProperties().Model;

            return container
                .RegisterType<IFinder, ODataFinder>(Lifetime.PerScope)
                .RegisterInstance(edmModel, Lifetime.PerScope);
        }
    }
}