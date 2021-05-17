
using System;
using System.Net.Http;

namespace UserInterface.Factories
{
	internal static class HttpClientFactory
	{
		internal static HttpClient Create()
		{
			return new HttpClient() { BaseAddress = new Uri("http://localhost:44376/api/") };
		}
	}
}
