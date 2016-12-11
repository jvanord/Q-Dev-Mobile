using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QDevMobile.Views
{
	public partial class Dashboard : ContentPage
	{
		private ViewModels.DashboardViewModel _viewModel;
		public Dashboard(ViewModels.DashboardViewModel model = null)
		{
			InitializeComponent();
			_viewModel = model;
			BindingContext = _viewModel;
		}

		public void OnTestButtonClicked(object sender, EventArgs e)
		{
			DisplayAlert("Authentication State", "IsAuthenticated:" + Auth.Current.IsAuthenticated, "Fine");
		}
	}
}
