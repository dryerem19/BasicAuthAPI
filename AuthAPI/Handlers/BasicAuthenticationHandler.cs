using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuthAPI.Handlers
{
	// Обрабатывает запросы аутентификации
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		public BasicAuthenticationHandler(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock
			)
			: base(options, logger, encoder, clock)
		{

		}

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
		{
			if (!Request.Headers.ContainsKey("Authorization"))
				return AuthenticateResult.Fail("Authorization header was not found");

			var authHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

			if (TokenContext.Tokens[authHeaderValue.Parameter] != null)
			{
				var claims = new[] { new Claim(ClaimTypes.Name, authHeaderValue.Parameter), new Claim(ClaimTypes.Name, 
					TokenContext.Tokens[authHeaderValue.Parameter]) };
				var identity = new ClaimsIdentity(claims, "BasicAuthentication");
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, "BasicAuthentication");

				return AuthenticateResult.Success(ticket);
			}
			else
			{
				return AuthenticateResult.Fail("Invalid login or password");
			}
		}
	}
}
