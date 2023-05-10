using Libraries.Authentication.Clients;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Libraries.Authentication.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("No Authorization key in headers"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Wrong authorization type"));
            }


            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));

            var headerSplitted = authBase64Decoded.Split(":");

            var clientLogin = headerSplitted[0];
            var clientPassword = headerSplitted[1];

            var section = _configuration.GetSection("BaseUser");

            if (clientLogin != section.GetValue<string>("Login") || clientPassword != section.GetValue<string>("Password"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Wrong combination of login and password"));
            }

            var client = new BasicAuthenticationClient
            {
                AuthenticationType = "Basic",
                IsAuthenticated = true,
                Name = clientLogin
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client,
                new[] { new Claim(ClaimTypes.Name, clientLogin) }
                ));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
