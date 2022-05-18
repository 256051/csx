

namespace IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// List of registered claims from different sources
    /// http://tools.ietf.org/html/rfc7519#section-4
    /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
    /// </summary>
    public struct JwtRegisteredClaimNames
    {
        /// <summary>
        /// </summary>
        public const string Actort = JsonWebTokens.JwtRegisteredClaimNames.Actort;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Acr = JsonWebTokens.JwtRegisteredClaimNames.Acr;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Amr = JsonWebTokens.JwtRegisteredClaimNames.Amr;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Aud = JsonWebTokens.JwtRegisteredClaimNames.Aud;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthTime = JsonWebTokens.JwtRegisteredClaimNames.AuthTime;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string Azp = JsonWebTokens.JwtRegisteredClaimNames.Azp;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Birthdate = JsonWebTokens.JwtRegisteredClaimNames.Birthdate;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken
        /// </summary>
        public const string CHash = JsonWebTokens.JwtRegisteredClaimNames.CHash;

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
        /// </summary>
        public const string AtHash = JsonWebTokens.JwtRegisteredClaimNames.AtHash;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Email = JsonWebTokens.JwtRegisteredClaimNames.Email;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Exp = JsonWebTokens.JwtRegisteredClaimNames.Exp;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Gender = JsonWebTokens.JwtRegisteredClaimNames.Gender;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string FamilyName = JsonWebTokens.JwtRegisteredClaimNames.FamilyName;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string GivenName = JsonWebTokens.JwtRegisteredClaimNames.GivenName;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Iat = JsonWebTokens.JwtRegisteredClaimNames.Iat;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Iss = JsonWebTokens.JwtRegisteredClaimNames.Iss;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Jti = JsonWebTokens.JwtRegisteredClaimNames.Jti;

        /// <summary>
        /// </summary>
        public const string NameId = JsonWebTokens.JwtRegisteredClaimNames.NameId;

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </summary>
        public const string Nonce = JsonWebTokens.JwtRegisteredClaimNames.Nonce;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Nbf = JsonWebTokens.JwtRegisteredClaimNames.Nbf;

        /// <summary>
        /// </summary>
        public const string Prn = JsonWebTokens.JwtRegisteredClaimNames.Prn;

        /// <summary>
        /// http://openid.net/specs/openid-connect-frontchannel-1_0.html#OPLogout
        /// </summary>
        public const string Sid = JsonWebTokens.JwtRegisteredClaimNames.Sid;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4
        /// </summary>
        public const string Sub = JsonWebTokens.JwtRegisteredClaimNames.Sub;

        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-5
        /// </summary>
        public const string Typ = JsonWebTokens.JwtRegisteredClaimNames.Typ;

        /// <summary>
        /// </summary>
        public const string UniqueName = JsonWebTokens.JwtRegisteredClaimNames.UniqueName;

        /// <summary>
        /// </summary>
        public const string Website = JsonWebTokens.JwtRegisteredClaimNames.Website;
    }
}
