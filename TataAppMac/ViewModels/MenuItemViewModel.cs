namespace TataAppMac.ViewModels
{
    using System.Windows.Input;
	using GalaSoft.MvvmLight.Command;
	using TataAppMac.Models;
	using TataAppMac.Serviices;

	public class MenuItemViewModel
    {
		#region Attributes
		NavigationService navigationService;
		DataService dataService;
		#endregion

		#region Properties
		public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Constructors
        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        async void Navigate()
        {
            if (PageName == "LoginPage")
            {
                var employee = dataService.First<Employee>(false);
                employee.IsRemembered = false;
                dataService.Update(employee);
                navigationService.SetMainPage(PageName);
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();

                switch (PageName)
                {
					case "TimesPage":
						mainViewModel.Times = new TimesViewModel();
						break;
					case "LocationsPage":
						mainViewModel.Locations = new LocationsViewModel();
						break;
					case "MyProfilePage":
						mainViewModel.MyProfile = new MyProfileViewModel();
						break;
					case "EmployeesPage":
						mainViewModel.Employees = new EmployeesViewModel();
						break;
				}

				await navigationService.Navigate(PageName);
			}
        }
        #endregion
    }
}
