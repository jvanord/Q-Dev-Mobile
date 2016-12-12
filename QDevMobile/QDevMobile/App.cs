using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QDevMobile.Services;
using QDevMobile.Views;
using Xamarin.Forms;

namespace QDevMobile
{
	public class App : Application
	{
		public App()
		{
			MainPage = Auth.Current.IsAuthenticated
				? new NavigationPage(new Dashboard())
				: new NavigationPage(new LoginPage());
			Current = this;
		}

		public static new App Current { get; private set; }

		public static ApiClient GetApiClient()
		{
			return Current.UseLocalApi 
				? new ApiClient("http://localhost:26938", Auth.Current.GetTokenIfAuthenticated()) 
				: new ApiClient("https://indasys-qdev.azurewebsites.net", Auth.Current.GetTokenIfAuthenticated());
		}

		public bool UseLocalApi { get; set; }

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
