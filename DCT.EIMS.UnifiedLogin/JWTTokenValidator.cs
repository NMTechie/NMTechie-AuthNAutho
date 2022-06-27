using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;

namespace DCT.EIMS.UnifiedLogin
{
    public class JWTTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JWTTokenValidator()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> ValidateTokenAsync(string securityToken)
        {
            // Read securityToken as a Valid JWT token
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadJwtToken(securityToken);

            // generate the .wellknown endpoint to read keys for validation
            var iss = token.Issuer;
            var policyid = string.Empty;
            if (token.Payload.ToList().Exists(x => x.Key == "tfp"))
            {
                policyid = token.Payload["tfp"].ToString();
            }
            else
            {
                policyid = token.Payload["acr"].ToString();
            }
            var metadataEndpoint = $"{iss}.well-known/openid-configuration?p={policyid}";
            var cm = new ConfigurationManager<OpenIdConnectConfiguration>(metadataEndpoint,
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever());
            var discoveryDocument = await cm.GetConfigurationAsync();
            var signingKeys = discoveryDocument.SigningKeys;

            // Configure the validation parameters for verifying the token
            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudiences = new string[] { UnifiedAuthConstant.B2CRegClientId },
                ValidateIssuer = true,
                ValidIssuers = new string[] { UnifiedAuthConstant.B2CTokenIssuer },
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = signingKeys
            };

            // Validate token and extract Claims principal if valid
            SecurityToken validatedToken = new JwtSecurityToken();
            ClaimsPrincipal claimsPrincipal = null;
            claimsPrincipal = _tokenHandler.ValidateToken(securityToken, tvp, out validatedToken);


            if (claimsPrincipal != null)
            {
                // print cliams attributes
                foreach (var claim in claimsPrincipal.Claims)
                {
                    Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
                }
                return true;
            }
            return false;
        }
    }
}
