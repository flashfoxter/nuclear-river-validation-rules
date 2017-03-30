﻿using System.Web.Http;

using NuClear.ValidationRules.Querying.Host.Composition;
using NuClear.ValidationRules.Querying.Host.DataAccess;
using NuClear.ValidationRules.SingleCheck;
using NuClear.ValidationRules.Storage.Model.Messages;

namespace NuClear.ValidationRules.Querying.Host.Controllers
{
    [RoutePrefix("api/SingleForCancel")]
    public class SingleForCancelController : ApiController
    {
        private readonly ValidationResultFactory _factory;
        private readonly ValidatorFactory _validatorFactory;

        public SingleForCancelController(ValidationResultFactory factory, ValidatorFactory validatorFactory)
        {
            _factory = factory;
            _validatorFactory = validatorFactory;
        }

        [Route(""), HttpPost]
        public IHttpActionResult Post([FromBody]ApiRequest request)
        {
            var pipeline = _validatorFactory.Create();
            var query = pipeline.Execute(request.OrderId);

            var messages = query.ToMessages(ResultType.SingleForCancel);
            var result = _factory.GetValidationResult(messages);
            return Ok(result);
        }

        public class ApiRequest
        {
            public long OrderId { get; set; }
        }
    }
}
