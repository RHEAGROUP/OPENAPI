﻿// -------------------------------------------------------------------------------------------------
// <copyright file="SecurityScheme.cs" company="RHEA System S.A.">
// 
//   Copyright 2022-2023 RHEA System S.A.
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
    /// Defines a security scheme that can be used by the operations.
    /// Supported schemes are HTTP authentication, an API key (either as a header, a cookie parameter or as a query parameter), mutual
    /// TLS (use of a client certificate), OAuth2’s common flows (implicit, password, client credentials and authorization code)
    /// as defined in [RFC6749], and OpenID Connect Discovery. Please note that as of 2020, the implicit flow is about to be
    /// deprecated by OAuth 2.0 Security Best Current Practice. Recommended for most use case is Authorization Code Grant flow with PKCE.
    /// </summary>
    /// <remarks>
    /// https://spec.openapis.org/oas/latest.html#security-scheme-object
    /// </remarks>
    public class SecurityScheme
    {
        /// <summary>
        /// REQUIRED. The type of the security scheme. Valid values are "apiKey", "http", "mutualTLS", "oauth2", "openIdConnect".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A description for security scheme. CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// REQUIRED. The name of the header, query or cookie parameter to be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// REQUIRED. The location of the API key. Valid values are "query", "header" or "cookie".
        /// </summary>
        public string In { get; set; }

        /// <summary>
        /// REQUIRED. The name of the HTTP Authorization scheme to be used in the Authorization header as defined in [RFC7235].
        /// The values used SHOULD be registered in the IANA Authentication Scheme registry.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// A hint to the client to identify how the bearer token is formatted. Bearer tokens are usually generated by an
        /// authorization server, so this information is primarily for documentation purposes.
        /// </summary>
        public string BearerFormat { get; set; }

        /// <summary>
        /// REQUIRED. An object containing configuration information for the flow types supported.
        /// </summary>
        public OAuthFlows Flows { get; set; }

        /// <summary>
        /// REQUIRED. OpenId Connect URL to discover OAuth2 configuration values. This MUST be in the form of a URL.
        /// The OpenID Connect standard requires the use of TLS.
        /// </summary>
        public string OpenIdConnectUrl { get; set; }
    }
}
