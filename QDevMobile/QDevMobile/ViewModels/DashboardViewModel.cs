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
		}

		public string WelcomeMessage { get; set; }
		public string DisplayName { get; private set; }

		public void LoadUserInfo()
		{

		}
	}
}
