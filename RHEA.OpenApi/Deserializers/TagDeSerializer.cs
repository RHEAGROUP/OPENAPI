﻿// -------------------------------------------------------------------------------------------------
// <copyright file="TagDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="TagDeSerializer"/> is to deserialize the <see cref="Tag"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#tag-object
    /// </remarks>
    internal class TagDeSerializer
    {
        /// <summary>
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<TagDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal TagDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.loggerFactory = loggerFactory;

            this.logger = this.loggerFactory == null ? NullLogger<TagDeSerializer>.Instance : this.loggerFactory.CreateLogger<TagDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Tag"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Tag"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Tag"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Tag"/> object
        /// </exception>
        internal Tag DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start TagDeSerializer.DeSerialize");

            var tag = new Tag();

            if (!jsonElement.TryGetProperty("name"u8, out JsonElement nameProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Tag.name property is not available, this is an invalid OpenAPI document");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Tag.name property is not available, this is an invalid OpenAPI document");
                }
            }
            else
            {
                tag.Name = nameProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description"u8, out JsonElement descriptionProperty))
            {
                tag.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("externalDocs"u8, out JsonElement externalDocsProperty))
            {
                var externalDocumentationDeSerializer = new ExternalDocumentationDeSerializer(this.loggerFactory);
                tag.ExternalDocs = externalDocumentationDeSerializer.DeSerialize(externalDocsProperty, strict);
            }

            this.logger.LogTrace("Finish TagDeSerializer.DeSerialize");

            return tag;
        }
    }
}
