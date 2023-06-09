﻿// -------------------------------------------------------------------------------------------------
// <copyright file="ReferenceDeSerializer.cs" company="RHEA System S.A.">
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
    /// The purpose of the <see cref="ReferenceDeSerializer"/> is to deserialize the <see cref="Reference"/> object
    /// from a <see cref="JsonElement"/>
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#reference-object
    /// </remarks>
    internal class ReferenceDeSerializer
    {
        /// <summary>
        /// The <see cref="ILogger"/> used to log
        /// </summary>
        private readonly ILogger<ReferenceDeSerializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDeSerializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The (injected) <see cref="ILoggerFactory"/> used to setup logging
        /// </param>
        internal ReferenceDeSerializer(ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger<ReferenceDeSerializer>.Instance : loggerFactory.CreateLogger<ReferenceDeSerializer>();
        }

        /// <summary>
        /// Deserializes an instance of <see cref="Reference"/> from the provided <paramref name="jsonElement"/>
        /// </summary>
        /// <param name="jsonElement">
        /// The <see cref="JsonElement"/> that contains the <see cref="Reference"/> json object
        /// </param>
        /// <param name="strict">
        /// a value indicating whether deserialization should be strict or not. If true, exceptions will be
        /// raised if a required property is missing. If false, a missing required property will be logged
        /// as a warning
        /// </param>
        /// <returns>
        /// an instance of <see cref="Reference"/>
        /// </returns>
        /// <exception cref="SerializationException">
        /// Thrown in case the <see cref="JsonElement"/> is not a valid OpenApi <see cref="Reference"/> object
        /// </exception>
        internal Reference DeSerialize(JsonElement jsonElement, bool strict)
        {
            this.logger.LogTrace("Start ReferenceDeSerializer.DeSerialize");

            var reference = new Reference();

            if (!jsonElement.TryGetProperty("$ref"u8, out JsonElement refProperty))
            {
                if (strict)
                {
                    throw new SerializationException("The REQUIRED Reference.$ref property is not available, this is an invalid Reference object");
                }
                else
                {
                    this.logger.LogWarning("The REQUIRED Reference.$ref property is not available, this is an invalid Reference object");
                }
            }
            else
            {
                reference.Ref = refProperty.GetString();
            }

            if (jsonElement.TryGetProperty("summary"u8, out JsonElement summaryProperty))
            {
                reference.Summary = summaryProperty.GetString();
            }

            if (jsonElement.TryGetProperty("description"u8, out JsonElement descriptionProperty))
            {
                reference.Description = descriptionProperty.GetString();
            }

            if (jsonElement.TryGetProperty("$comment"u8, out JsonElement commentProperty))
            {
                reference.Comments = commentProperty.GetString();
            }

            this.logger.LogTrace("Finish ReferenceDeSerializer.DeSerialize");

            return reference;
        }
    }
}
