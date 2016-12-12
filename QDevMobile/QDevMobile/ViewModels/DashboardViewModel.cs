using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDevMobile.ViewModels
{
	public class DashboardViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public DashboardViewModel()
		{
			WelcomeMessage = "Yeah, wadya want?";
			if (Auth.Current.IsAuthenticated)
				LoadUserInfo();
		}

		public string WelcomeMessage { get; set; }
		public string DisplayName { get; private set; }
		public UserInfo UserInfo { get; private set; }

		protected async void LoadUserInfo()
		{
			UserInfo = await App.GetApiClient().Get("/api/users").CallAndGetDataAsync<UserInfo>();
			PropertyChanged(this, new PropertyChangedEventArgs("UserInfo"));
			var otherInfo = await App.GetApiClient().Get("/api/me").CallAndGetDataAsync<UserInfo>();
			DisplayName = otherInfo == null ? "Error" : otherInfo.DisplayName;
			PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
		}
	}
}
