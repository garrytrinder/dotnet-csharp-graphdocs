using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GraphDocsConnector
{
    internal class TokenValidator
    {
        private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
        private string _audience;
        private string _issuer;

        public TokenValidator(string authority, string audience)
        {
            _audience = audience;
            _issuer = authority;
            var documentRetriever = new HttpDocumentRetriever(Utils.GetHttpClient())
            {
                RequireHttps = authority.StartsWith("https://")
            };

            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{authority}.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                documentRetriever
            );
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var discoveryDocument = await _configurationManager.GetConfigurationAsync(CancellationToken.None);
                var signingKeys = discoveryDocument.SigningKeys;

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = signingKeys,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30) // Allow a brief skew in time
                };

                try
                {
                    var principal = new JwtSecurityTokenHandler()
                        .ValidateToken(token, validationParameters, out var validatedToken);

                    return validatedToken != null;
                }
                catch (SecurityTokenValidationException)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
