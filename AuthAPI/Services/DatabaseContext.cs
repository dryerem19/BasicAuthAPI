using AuthAPI.Models;
using System.Collections.Generic;

namespace AuthAPI.Services
{
	public class DatabaseContext : IDatabaseContext
	{
		public List<LoginModel> Logins => new List<LoginModel>
		{
			new LoginModel { Login = "admin", Password = "qwerty" },
			new LoginModel { Login = "dryerem19", Password = "mypassword" },
		};
	}
}
