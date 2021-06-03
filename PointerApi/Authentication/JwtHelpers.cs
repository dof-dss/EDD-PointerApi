using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PointerApi.Data;
using PointerApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace PointerApi.Authentication
{
    public class JwtHelpers
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const double ISSUED_WITHIN_SECONDS = 30;

        public JwtHelpers(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<SecurityKey> ResolveSigningKey(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        {
            var signingKey = new List<SecurityKey>();

            var jwtToken = securityToken as JwtSecurityToken;

            var kidFromJwt = "";

            if (jwtToken.Payload.ContainsKey("kid"))
                kidFromJwt = jwtToken.Payload["kid"].ToString();

            if (string.IsNullOrEmpty(kidFromJwt)) return signingKey;

            var secretKey = GetSigningKeyFromDatabase(kidFromJwt);

            if (string.IsNullOrEmpty(secretKey)) return signingKey;

            signingKey.Add(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)));

            return signingKey;
        }


        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var jwtToken = securityToken as JwtSecurityToken;

            return IssuedWithin(jwtToken);
        }


        private static bool IssuedWithin(JwtSecurityToken jwtToken)
        {
            var iatValid = false;

            if (jwtToken.Payload.ContainsKey("iat"))
            {
                var issuedString = jwtToken.Payload["iat"].ToString();

                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedString));

                var issuedDate = dateTimeOffset.DateTime;

                var diff = DateTime.UtcNow.Subtract(issuedDate);

                if (diff.TotalSeconds < ISSUED_WITHIN_SECONDS) iatValid = true;
            }

            return iatValid;
        }

        private string GetSigningKeyFromDatabase(string kidFromJwt)
        {
            ConsumingApplication consumingApplication = null;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<PointerContext>();

                consumingApplication = dbContext.ConsumingApplication.FirstOrDefault(x => x.ApiKey == kidFromJwt && !x.IsDisabled);
            }

            if (consumingApplication != null && consumingApplication.SecretKey != null)
            {
                return consumingApplication.SecretKey;
            }
            else return "";
        }
    }
}
