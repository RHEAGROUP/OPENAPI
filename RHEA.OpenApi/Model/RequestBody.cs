﻿// -------------------------------------------------------------------------------------------------
// <copyright file="RequestBody.cs" company="RHEA System S.A.">
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

namespace OpenApi.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes a single request body.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#request-body-object
    /// </remarks>
    public class RequestBody
    {
        /// <summary>
        /// A brief description of the request body. This could contain examples of use. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The content of the request body. The key is a media type or media type range and the value describes it.
        /// For requests that match multiple keys, only the most specific key is applicable. e.g. text/plain overrides text/*
        /// </summary>
        public Dictionary<string, MediaType> Content { get; set; } = new Dictionary<string, MediaType>();

        /// <summary>
        /// Determines if the request body is required in the request. Defaults to false.
        /// </summary>
        public bool Required { get; set; }
    }
}
