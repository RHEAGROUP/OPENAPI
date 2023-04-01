﻿// -------------------------------------------------------------------------------------------------
// <copyright file="Header.cs" company="RHEA System S.A.">
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
    /// <summary>
    /// The Header Object follows the structure of the Parameter Object with the following changes:
    ///   1. name MUST NOT be specified, it is given in the corresponding headers map.
    ///   2. in MUST NOT be specified, it is implicitly in header.
    ///   3. All traits that are affected by the location MUST be applicable to a location of header (for example, style).
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#header-object
    /// </remarks>
    public class Header
    {
        /// <summary>
        /// A brief description of the parameter. This could contain examples of use. CommonMark syntax MAY be used for rich text representation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines whether this parameter is mandatory. If the parameter location is "path", this property is REQUIRED and its value MUST be true.
        /// Otherwise, the property MAY be included and its default value is false.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Specifies that a parameter is deprecated and SHOULD be transitioned out of usage. Default value is false.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Sets the ability to pass empty-valued parameters. This is valid only for query parameters and allows sending a parameter with an empty value.
        /// Default value is false. If style is used, and if behavior is n/a (cannot be serialized), the value of allowEmptyValue SHALL be ignored.
        /// Use of this property is NOT RECOMMENDED, as it is likely to be removed in a later revision.
        /// </summary>
        public bool AllowEmptyValue { get; set; }
    }
}
