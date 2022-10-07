using AuthAPI.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services
{
	public class TokenContext
	{
		private static Dictionary<string, string> _tokens = new Dictionary<string, string>();
		public static Dictionary<string, string> Tokens
		{
			get
			{
				return _tokens;
			}
			set
			{
				_tokens = value;
			}
		}

		public static string Generate(LoginModel login)
		{
			StringBuilder builder = new StringBuilder();

			using (SHA256 hash = SHA256.Create())
			{
				Encoding encoding = Encoding.UTF8;
				byte[] result = hash.ComputeHash(encoding.GetBytes($"{login.Login}:{login.Password}"));

				foreach (byte b in result)
				{
					builder.Append(b.ToString("x2"));
				}
			}

			return builder.ToString();
		}
	}
}
