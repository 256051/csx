using System;
using System.Security.Claims;
using System.Xml;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Defines the interface for a Security Token Handler.
    /// </summary>
    public abstract class SecurityTokenHandler : TokenHandler, ISecurityTokenValidator
    { 
    
    }
}
