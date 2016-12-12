using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QDevMobile
{
	public class Auth
	{
		public enum AuthenticationMethod { Empty, Database, Facebook, Google }

		private static Context _currentContext;
		public static Context Current { get { _currentContext = _currentContext ?? new Context(); return _currentContext; } }

		public class Context
		{
			private string token;
			private DateTime? tokenExpires;

			public async Task LoginWithDatabase(string username, string password, bool useLocal = false)
			{
				Method = AuthenticationMethod.Database;
				var loginResponse = await App.GetApiClient().Post("/token")
					.Data("grant_type", "password")
					.Data("username", username)
					.Data("password", password)
					.CallAsync();
				var tokenData = await loginResponse.GetDataAsync<TokenResponse>();
				if (tokenData == null) return;
				token = tokenData.AccessToken;
				tokenExpires = DateTime.Now.AddMilliseconds(tokenData.ExpiresIn);
			}

			public string GetTokenIfAuthenticated()
			{
				if (token == string.Empty) token = null;
				return token;
			}

			public AuthenticationMethod Method { get; private set; }
			public bool IsAuthenticated
			{
				get
				{
					if (string.IsNullOrEmpty(token))
					{
						token = null;
						return false;
					}
					return tokenExpires.HasValue ? tokenExpires.Value > DateTime.Now : true;
				}
			}

			public void Logout() { token = null; tokenExpires = null; }
		}

		public class TokenResponse
		{
			[JsonProperty("access_token")]
			public string AccessToken { get; set; }

			[JsonProperty("expires_in")]
			public int ExpiresIn { get; set; }
		}
	}
}
