using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QDevMobile.Views
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		public async void OnLoginClick(object sender, EventArgs e)
		{
			try
			{
				App.Current.UseLocalApi = UseLocalOption.IsToggled;
				await Auth.Current.LoginWithDatabase(UsernameEntry.Text, PasswordEntry.Text);
				if (Auth.Current.IsAuthenticated)
				{
					Navigation.InsertPageBefore(new Dashboard(new ViewModels.DashboardViewModel { WelcomeMessage = "Hello chap. How can we help?" }), this);
					await Navigation.PopAsync();
				}
				else
				{
					ErrorLabel.Text = "Login Failure";
				}
			}
			catch (Exception ex)
			{
				ErrorLabel.Text = ex.Message;
			}
		}
	}
}
