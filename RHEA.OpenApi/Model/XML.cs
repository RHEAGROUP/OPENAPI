﻿// -------------------------------------------------------------------------------------------------
// <copyright file="XML.cs" company="RHEA System S.A.">
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
    /// A metadata object that allows for more fine-tuned XML model definitions.
    /// When using arrays, XML element names are not inferred (for singular/plural forms) and the name property SHOULD
    /// be used to add that information. See examples for expected behavior.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#xml-object
    /// </remarks>
    public class XML
    {
        /// <summary>
        /// Replaces the name of the element/attribute used for the described schema property. When defined within items,
        /// it will affect the name of the individual XML elements within the list. When defined alongside type being array (outside the items),
        /// it will affect the wrapping element and only if wrapped is true. If wrapped is false, it will be ignored.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URI of the namespace definition. This MUST be in the form of an absolute URI.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// The prefix to be used for the name.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Declares whether the property definition translates to an attribute instead of an element. Default value is false.
        /// </summary>
        public bool Attribute { get; set; }

        /// <summary>
        /// MAY be used only for an array definition. Signifies whether the array is wrapped (for example, <books><book/><book/></books>) or unwrapped
        /// (<book/><book/>). Default value is false. The definition takes effect only when defined alongside type being array (outside the items).
        /// </summary>
        public bool Wrapped { get; set; }
    }
}
