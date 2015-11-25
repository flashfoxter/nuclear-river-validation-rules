﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;

using NuClear.AdvancedSearch.Common.Metadata.Elements;
using NuClear.AdvancedSearch.Common.Metadata.Identities;
using NuClear.CustomerIntelligence.Domain;
using NuClear.Metamodeling.Elements.Identities.Builder;
using NuClear.Metamodeling.Processors;
using NuClear.Metamodeling.Provider;
using NuClear.Metamodeling.Provider.Sources;
using NuClear.Querying.OData.Building;

using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NuClear.CustomerIntelligence.Querying.Tests
{
    public class EdmModelBuilderTests
    {
        [Test]
        public void ShouldBuildValidModelForCustomerIntelligenceContext()
        {
            var provider = CreateProvider(new QueryingMetadataSource());
            var contextId = Metadata.Id.For<QueryingMetadataIdentity>("CustomerIntelligence");

            BoundedContextElement boundedContext;
            provider.TryGetMetadata(contextId, out boundedContext);

            var model = BuildModel(provider, contextId);

            Assert.That(model, Is.Not.Null.And.Matches(ModelConstraints.IsValid));
        }

        private static IMetadataProvider CreateProvider(params IMetadataSource[] sources)
        {
            return new MetadataProvider(sources, new IMetadataProcessor[0]);
        }

        private static IEdmModel BuildModel(IMetadataProvider provider, Uri contextId)
        {
            var builder = new EdmModelBuilder(provider);
            var model = builder.Build(contextId);

            model.Dump();

            return model;
        }
    }

    internal static class EdmModelExtensions
    {
        [Conditional("DEBUG")]
        public static void Dump(this IEdmModel model)
        {
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
            {
                IEnumerable<EdmError> errors;
                EdmxWriter.TryWriteEdmx(model, writer, EdmxTarget.OData, out errors);
            }

            Debug.WriteLine(sb.ToString());
        }
    }

    internal static class ModelConstraints
    {
        public static Constraint IsValid
        {
            get
            {
                return new ModelValidationConstraint();
            }
        }

        private class ModelValidationConstraint : Constraint
        {
            private const int MaxErrorsToDisplay = 5;
            private IEnumerable<EdmError> _errors;

            public override bool Matches(object actual)
            {
                var model = actual as IEdmModel;
                if (model == null)
                {
                    throw new ArgumentException("The specified actual value is not a model.", "actual");
                }

                return model.Validate(out _errors);
            }

            public override void WriteDescriptionTo(MessageWriter writer)
            {
                writer.WriteCollectionElements(_errors, 0, MaxErrorsToDisplay);
            }
        }
    }
}