using System;
using TataAppMac.ViewModels;
using Xamarin.Forms;

namespace TataAppMac.Views
{
    public partial class TimesPage : ContentPage
    {
        public TimesPage()
        {
            InitializeComponent();

			var timesViewModel = TimesViewModel.GetInstance();
			base.Appearing += (object sender, EventArgs e) =>
			{
				timesViewModel.RefreshCommand.Execute(this);
			};
		}
    }
}
