namespace TataAppMac.Views
{
    using System;
	using TataAppMac.ViewModels;
	using Xamarin.Forms;

	public partial class EmployeesPage : ContentPage
    {
        public EmployeesPage()
        {
            InitializeComponent();

			var employeesViewModel = EmployeesViewModel.GetInstance();
			base.Appearing += (object sender, EventArgs e) =>
			{
				employeesViewModel.RefreshCommand.Execute(this);
			};
		}
    }
}
