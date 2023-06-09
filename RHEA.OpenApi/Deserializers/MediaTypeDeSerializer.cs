﻿// -------------------------------------------------------------------------------------------------
// <copyright file="MediaTypeDeSerializer.cs" company="RHEA System S.A.">
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
    using System.Runtime.Serialization;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using OpenApi.Model;

    /// <summary>
    /// The purpose of the <see cref="MediaTypeDeSerializer"/> is to deserialize the <see cref="MediaType"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#media-type-object
    /// </remarks>
    internal class MediaTypeDeSerializer : ReferencerDeserializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<MediaTypeDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeDeSerializer"/> class.
        /// </summary>
        /// <param name="referenceResolver">
        /// The <see cref="ReferenceResolver"/> that is used to register any <see cref="ReferenceInfo"/> objects
        /// and later resolve them
        /// </param>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal MediaTypeDeSerializer(ReferenceResolver referenceResolver, ILoggerFactory loggerFactory = null) 
            : base(referenceResolver)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<MediaTypeDeSerializer>.Instance : this.loggerFactory.CreateLogger<MediaTypeDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="MediaType"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="MediaType"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="MediaType"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="MediaType"/> object
        /// </exception>
        internal MediaType DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start MediaTypeDeSerializer.DeSerialize");

            var mediaType = new MediaType();

            if (jsonElement.TryGetProperty("schema"u8, out JsonElement schemaProperty))
            {
                if (schemaProperty.TryGetProperty("$ref"u8, out var referenceElement))
                {
                    var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);
                    var reference = referenceDeSerializer.DeSerialize(schemaProperty, strict);
                    mediaType.SchemaReference = reference;
                    this.Register(reference, mediaType, "schema");
                }
                else
                {
                    var schemaDeSerializer = new SchemaDeSerializer(this.referenceResolver, this.loggerFactory);
                    mediaType.Schema = schemaDeSerializer.DeSerialize(schemaProperty, strict);
                }
            }

            if (jsonElement.TryGetProperty("example"u8, out JsonElement exampleProperty))
            {
                mediaType.Example = exampleProperty.ToString();
            }

            this.DeserializeExamples(jsonElement, mediaType, strict);

            this.DeserializeEncoding(jsonElement, mediaType, strict);

            this.logger.LogTrace("Finish MediaTypeDeSerializer.DeSerialize");

            return mediaType;
        }

        /// <summary>
        /// Deserializes the web hook <see cref="Encoding"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="MediaType"/> json object
        /// </param>
        /// <param name="mediaType">
        /// The <see cref="MediaType"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="MediaType"/> object
        /// </exception>
        private void DeserializeExamples(JsonElement jsonElement, MediaType mediaType, bool strict)
        {
            if (jsonElement.TryGetProperty("examples"u8, out JsonElement examplesProperty))
            {
                var exampleDeSerializer = new ExampleDeSerializer(this.loggerFactory);
                var referenceDeSerializer = new ReferenceDeSerializer(this.loggerFactory);

                foreach (var itemProperty in examplesProperty.EnumerateObject())
                {
                    if (itemProperty.Value.TryGetProperty("$ref"u8, out var referenceElement))
                    {
                        var reference = referenceDeSerializer.DeSerialize(itemProperty.Value, strict);
                        mediaType.ExamplesReferences.Add(itemProperty.Name, reference);
                        this.Register(reference, mediaType, "Examples", itemProperty.Name);
                    }
                    else
                    {
                        var example = exampleDeSerializer.DeSerialize(itemProperty.Value);
                        mediaType.Examples.Add(itemProperty.Name, example);
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes the web hook <see cref="Encoding"/>s from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="MediaType"/> json object
        /// </param>
        /// <param name="mediaType">
        /// The <see cref="MediaType"/> that is being deserialized
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="MediaType"/> object
        /// </exception>
        private void DeserializeEncoding(JsonElement jsonElement, MediaType mediaType, bool strict)
        {
            if (jsonElement.TryGetProperty("encoding"u8, out JsonElement encodingProperty))
            {
                var encodingDeSerializer = new EncodingDeSerializer(this.referenceResolver, this.loggerFactory);

                foreach (var e in encodingProperty.EnumerateObject())
                {
                    var encodingName = e.Name;

                    var encoding = encodingDeSerializer.DeSerialize(e.Value, strict);

                    mediaType.Encoding.Add(encodingName, encoding);
                }
            }
        }
    }
}
