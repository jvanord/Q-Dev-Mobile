using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QDevMobile.Services
{
	public class ApiClient
	{
		private string _baseUri;
		private string _uriPath;
		private SupportedMethod _method;
		private string _bearerToken;

		public ApiClient(string baseUri, string bearerToken = null) { _baseUri = baseUri; _bearerToken = bearerToken; }

		public enum SupportedMethod { Get, Post }

		public Dictionary<string, string> RequestData { get; private set; }

		public ApiClient Post(string uriPath)
		{
			_method = SupportedMethod.Post;
			_uriPath = uriPath;
			return this;
		}

		public ApiClient Get(string uriPath)
		{
			_method = SupportedMethod.Get;
			_uriPath = uriPath;
			return this;
		}

		public ApiClient Data(string key, string value)
		{
			if (RequestData == null) RequestData = new Dictionary<string, string>();
			RequestData.Add(key, value);
			return this;
		}

		public async Task<Response> CallAsync()
		{
			System.Diagnostics.Debug.WriteLine("Calling API: ({0}) {1}{2}", _method, _baseUri, _uriPath);
			RequestData = RequestData ?? new Dictionary<string, string>();
			using (var request = new HttpClient())
			{
				if (!string.IsNullOrEmpty(_bearerToken))
					request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
				HttpResponseMessage responseMessage;
				switch (_method)
				{
					case SupportedMethod.Post:
						var content = new FormUrlEncodedContent(RequestData.ToArray());
						content.Headers.Add("Authentication", "bearer " + _bearerToken);
						responseMessage = await request.PostAsync(_baseUri + _uriPath, content);
						break;
					case SupportedMethod.Get:
						var uri = new Uri(_baseUri + _uriPath);
						// TODO: Convert RequestData to QueryString
						responseMessage = await request.GetAsync(uri);
						break;
					default:
						throw new NotSupportedException("The HTTP method is not supported.");
				}
				return new Response(responseMessage);
			}

		}

		public async Task<T> CallAndGetDataAsync<T>()
		{
			var response = await CallAsync();
			return await response.GetDataAsync<T>();
		}

		public class Response
		{
			public Response(HttpResponseMessage response)
			{
				Message = response;
			}

			public HttpResponseMessage Message { get; private set; }
			public bool Success { get { return Message != null && Message.IsSuccessStatusCode; } }
			public async Task<T> GetDataAsync<T>()
			{
				if (!Success) return default(T);
				var responseContent = await Message.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(responseContent);


			}
		}
	}
}
