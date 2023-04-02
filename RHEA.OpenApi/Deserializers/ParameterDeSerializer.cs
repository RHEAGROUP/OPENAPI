﻿// -------------------------------------------------------------------------------------------------
// <copyright file="ParameterDeSerializer.cs" company="RHEA System S.A.">
// 
//   Copyright 2023 RHEA System S.A.
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace OpenApi.Deserializers
{
    using System.Data.Common;
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging.Abstractions;
    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="ParameterDeSerializer"/> is to deserialize the <see cref="Parameter"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#parameter-object
    /// </remarks>
    internal class ParameterDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ParameterDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ParameterDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<ParameterDeSerializer>.Instance : this.loggerFactory.CreateLogger<ParameterDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Parameter"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter"/> json object
        /// </param>
        /// <returns></returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter"/> object
        /// </exception>
        internal Parameter DeSerialize(JsonElement jsonElement)
        {
            var parameter = new Parameter();

            if (!jsonElement.TryGetProperty("name", out JsonElement nameProperty))
            {
                throw new SerializationException("The REQUIRED Parameter.name property is not available, this is an invalid Parameter object");
            }

            parameter.Name = nameProperty.GetString();

            if (!jsonElement.TryGetProperty("in", out JsonElement inProperty))
            {
                throw new SerializationException("The REQUIRED Parameter.in property is not available, this is an invalid Parameter object");
            }

            parameter.In = inProperty.GetString();

            if (jsonElement.TryGetProperty("description", out JsonElement descriptionProperty))
            {
                parameter.Description = descriptionProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.description property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("required", out JsonElement requiredProperty))
            {
                parameter.Required = requiredProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.required property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("deprecated", out JsonElement deprecatedProperty))
            {
                parameter.Deprecated = deprecatedProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.deprecated property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("allowEmptyValue", out JsonElement allowEmptyValueProperty))
            {
                parameter.AllowEmptyValue = allowEmptyValueProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.allowEmptyValue property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("style", out JsonElement styleProperty))
            {
                parameter.Style = styleProperty.GetString();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.description property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("explode", out JsonElement explodeProperty))
            {
                parameter.Explode = explodeProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.explode property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("allowReserved", out JsonElement allowReservedProperty))
            {
                parameter.AllowReserved = allowReservedProperty.GetBoolean();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.allowReserved property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("schema", out JsonElement schemaProperty))
            {
                var schemaDeSerializer = new SchemaDeSerializer(this.loggerFactory);
                parameter.Schema = schemaDeSerializer.DeSerialize(schemaProperty);
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.schema property is not provided in the OpenApi document");
            }

            if (jsonElement.TryGetProperty("example", out JsonElement exampleProperty))
            {
                parameter.Example = exampleProperty.ToString();
            }
            else
            {
                this.logger.LogTrace("The optional Parameter.example property is not provided in the OpenApi document");
            }

            this.DeserializeExamples(jsonElement, parameter);

            this.DeserializeContent(jsonElement, parameter);

            return parameter;
        }

        /// <summary>
        /// Deserializes the Parameter  <see cref="Example"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter "/> json object
        /// </param>
        /// <param name="parameter">
        /// The <see cref="Parameter "/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter "/> object
        /// </exception>
        private void DeserializeExamples(JsonElement jsonElement, Parameter parameter)
        {
            if (jsonElement.TryGetProperty("examples", out JsonElement examplesProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in examplesProperty.EnumerateObject())
                {
                    var key = itemProperty.Name;

                    foreach (var value in itemProperty.Value.EnumerateObject())
                    {
                        if (value.Name == "$ref")
                        {
                            var reference = referenceDeSerializer.DeSerialize(itemProperty.Value);
                            parameter.ExamplesReferences.Add(key, reference);
                        }
                        else
                        {
                            var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                            parameter.Examples.Add(key, example);
                        }
                    }
                }
            }
            else
            {
                this.logger.LogTrace("The optional Header.example property is not provided in the OpenApi document");
            }
        }

        /// <summary>
        /// Deserializes the Parameter.Content <see cref="MediaType"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Parameter"/> json object
        /// </param>
        /// <param name="parameter">
        /// The <see cref="Parameter"/> that is being deserialized
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Parameter"/> object
        /// </exception>
        private void DeserializeContent(JsonElement jsonElement, Parameter parameter)
        {
            if (jsonElement.TryGetProperty("content", out JsonElement contentProperty))
            {
                var mediaTypeDeSerializer = new MediaTypeDeSerializer(this.loggerFactory);

                foreach (var x in contentProperty.EnumerateObject())
                {
                    var mediaTypeName = x.Name;

                    var mediaType = mediaTypeDeSerializer.DeSerialize(x.Value);

                    parameter.Content.Add(mediaTypeName, mediaType);
                }
            }
            else
            {
                this.logger.LogTrace("The optional Header.content property is not provided in the OpenApi document");
            }
        }
    }
}
