using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;

namespace AuthAPI.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class SimpleController : ControllerBase
	{
		private readonly DatabaseContext mContext;

		public SimpleController(DatabaseContext context)
		{
			mContext = context;
		}

		[HttpGet]
		public string Get()
		{
			return "Hello, World!";
		}

		[AllowAnonymous]
		[HttpPost]
		public string Auth(string payload)
		{
			string[] credentials = Encoding.UTF8.GetString(Convert.FromBase64String(payload)).Split(":");

			if (null != mContext.Logins.Where(x => x.Login == credentials[0] && x.Password == credentials[1]).FirstOrDefault())
			{
				string token = TokenContext.Generate(new Models.LoginModel { Login = credentials[0], Password = credentials[1] });
				TokenContext.Tokens.Add(token, credentials[0]);

				return token;
			}

			Response.StatusCode = 401;
			return string.Empty;
		}
	}
}
	