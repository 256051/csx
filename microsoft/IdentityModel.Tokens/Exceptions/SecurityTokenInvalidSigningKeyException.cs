//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Throw this exception when a received Security Token has an invalid issuer signing key.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidSigningKeyException : SecurityTokenValidationException
    {
        /// <summary>
        /// Gets or sets the SigningKey that was found invalid.
        /// </summary>
        public SecurityKey SigningKey { get; set; }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException()
            : base("SecurityToken has invalid issuer signing key.")
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of  <see cref="SecurityTokenInvalidSigningKeyException"/>
        /// </summary>
        public SecurityTokenInvalidSigningKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidSigningKeyException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SecurityTokenInvalidSigningKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
