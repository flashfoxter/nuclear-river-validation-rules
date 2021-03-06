﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ValidationRules.Replication.Comparison.Tests.RiverService
{
    [DataContract]
    public class RiverSingleCheckRequest
    {
        [DataMember]
        public long OrderId { get; set; }
    }

    [DataContract]
    public class RiverMassCheckRequest
    {
        [DataMember]
        public long[] OrderIds { get; set; }

        [DataMember]
        public long ProjectId { get; set; }

        [DataMember]
        public DateTime ReleaseDate { get; set; }
    }

    [DataContract]
    public class RiverValidationResult
    {
        [DataMember]
        public int Rule { get; set; }

        [DataMember]
        public string Template { get; set; }

        [DataMember]
        public EntityReference[] References { get; set; }

        [DataMember]
        public EntityReference MainReference { get; set; }
    }

    [DataContract]
    public class EntityReference
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long Id { get; set; }
    }

    [ServiceContract]
    public interface IRiverRestService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/Single?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] Single(RiverSingleCheckRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/SingleForCancel?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] SingleForCancel(RiverSingleCheckRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/Manual?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] Manual(RiverMassCheckRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/ManualWithAccount?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] ManualWithAccount(RiverMassCheckRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/Prerelease?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] Prerelease(RiverMassCheckRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/Release?culture=ru-RU", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RiverValidationResult[] Release(RiverMassCheckRequest request);
    }

    [System.Diagnostics.DebuggerStepThrough]
    public class RiverRestService : ClientBase<IRiverRestService>, IRiverRestService
    {
        public RiverRestService(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public RiverValidationResult[] Single(RiverSingleCheckRequest request)
        {
            return base.Channel.Single(request);
        }

        public RiverValidationResult[] SingleForCancel(RiverSingleCheckRequest request)
        {
            return base.Channel.SingleForCancel(request);
        }

        public RiverValidationResult[] Manual(RiverMassCheckRequest request)
        {
            return base.Channel.Manual(request);
        }

        public RiverValidationResult[] ManualWithAccount(RiverMassCheckRequest request)
        {
            return base.Channel.ManualWithAccount(request);
        }

        public RiverValidationResult[] Prerelease(RiverMassCheckRequest request)
        {
            return base.Channel.Prerelease(request);
        }

        public RiverValidationResult[] Release(RiverMassCheckRequest request)
        {
            return base.Channel.Release(request);
        }
    }
}
