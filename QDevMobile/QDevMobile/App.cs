using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		}

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
