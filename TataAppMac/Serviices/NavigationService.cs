namespace TataAppMac.Serviices
{
    using System.Threading.Tasks;
	using TataAppMac.Views;

	public class NavigationService
    {
		public void SetMainPage(string pageName)
		{
			switch (pageName)
			{
				case "MasterPage":
					App.Current.MainPage = new MasterPage();
					break;
				case "LoginPage":
					App.Current.MainPage = new LoginPage();
					break;
				case "NewEmployeePage":
					App.Current.MainPage = new NewEmployeePage();
					break;
				case "ForgotPasswordPage":
					App.Current.MainPage = new ForgotPasswordPage();
					break;
				case "LoginFacebookPage":
					App.Current.MainPage = new LoginFacebookPage();
					break;
				default:
					break;
			}
		}

		public async Task Navigate(string pageName)
		{
			App.Master.IsPresented = false;

			switch (pageName)
			{
				case "TimesPage":
					await App.Navigator.PushAsync(new TimesPage());
					break;
				case "NewTimePage":
					await App.Navigator.PushAsync(new NewTimePage());
					break;
				case "LocationsPage":
					await App.Navigator.PushAsync(new LocationsPage());
					break;
				case "MyProfilePage":
					await App.Navigator.PushAsync(new MyProfilePage());
					break;
				case "ChangePasswordPage":
					await App.Navigator.PushAsync(new ChangePasswordPage());
					break;
				case "EmployeesPage":
					await App.Navigator.PushAsync(new EmployeesPage());
					break;
			}
		}

		public async Task Back()
		{
			await App.Navigator.PopAsync();
		}
    }
}