using System;
using System.Collections.ObjectModel;
using TataAppMac.Models;

namespace TataAppMac.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public Employee Employee
        {
            get;
            set;
        }

        public LoginViewModel Login
        {
            get;
            set;
        }

        public TimesViewModel Times
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;

            Menu = new ObservableCollection<MenuItemViewModel>();
            Login = new LoginViewModel();
            LoadMenu();
        }
		#endregion

		#region Singleton
		private static MainViewModel instance;

		public static MainViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new MainViewModel();
			}

			return instance;
		}
		#endregion        

        #region Methods
		private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Title = "Register Time",
                Icon = "ic_access_alarms.png",
                PageName = "TimesPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Sickleaves",
                Icon = "ic_favorite.png",
                PageName = "SickleavesPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Localizate Employees",
                Icon = "ic_location_on.png",
                PageName = "LocalizatePage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Close Sesion",
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
            });
        }
        #endregion
    }
}
