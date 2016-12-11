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

			public async void LoginWithDatabase(string username, string password)
			{
				Method = AuthenticationMethod.Database;
				using (var request = new HttpClient())
				{
					var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>("grant_type", "password"),
						new KeyValuePair<string, string>("username", username),
						new KeyValuePair<string, string>("password", password)
					});
					//var message = new HttpRequestMessage { Content = content, Method = HttpMethod.Post, RequestUri = new Uri("") };
					//message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("appliation/json"));
					//message.Headers.Add("Content-Type", "");
					var response = await request.PostAsync("http://localhost:26938/token", content);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						var authenticationTicket = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

						if (authenticationTicket != null)
						{
							token = authenticationTicket.AccessToken;
							tokenExpires = DateTime.Now.AddMilliseconds(authenticationTicket.ExpiresIn);
						}
					}
				}
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
