namespace TataAppMac.ViewModels
{
	using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
	using GalaSoft.MvvmLight.Command;
    using TataAppMac.Interfaces;
    using TataAppMac.Models;
	using TataAppMac.Serviices;
    using Xamarin.Forms;

    public class MainViewModel : INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		NavigationService navigationService;
        Employee employee;
        #endregion

        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public Employee Employee
        {
			set
			{
				if (employee != value)
				{
					employee = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employee"));
				}
			}
			get
			{
				return employee;
			}
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

        public NewTimeViewModel NewTime
        {
            get;
            set;
        }

        public LocationsViewModel Locations
        {
            get;
            set;
        }

		public MyProfileViewModel MyProfile
		{
			get;
			set;
		}

		public ChangePasswordViewModel ChangePassword
		{
			get;
			set;
		}

        public NewEmployeeViewModel NewEmployee
        {
            get;
            set;
        }

        public ForgotPasswordViewModel ForgotPassword
        {
            get;
            set;
        }

        public EmployeesViewModel Employees
        {
            get;
            set;
        }

        public EmployeeDetailViewModel EmployeeDetail
        {
            get;
            set;
        }

        public TimeDetailViewModel TimeDetail
        {
            get;
            set;
        }
		#endregion

		#region Constructors
		public MainViewModel()
        {
            instance = this;

            navigationService = new NavigationService();

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
		public void RegisterDevice()
		{
			var register = DependencyService.Get<IRegisterDevice>();
			register.RegisterDevice();
		}

		void LoadMenu()
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
				Title = "Locations",
				Icon = "ic_location_on.png",
				PageName = "LocationsPage",
			});

			Menu.Add(new MenuItemViewModel
			{
				Title = "My Profile",
				Icon = "ic_phonelink_setup.png",
				PageName = "MyProfilePage",
			});

			Menu.Add(new MenuItemViewModel
			{
				Title = "Employees",
				Icon = "ic_action_people.png",
				PageName = "EmployeesPage",
			});

			Menu.Add(new MenuItemViewModel
            {
                Title = "Close Sesion",
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
            });
        }
        #endregion

        #region Commands
        public ICommand NewTimeCommand
        {
            get { return new RelayCommand(GoNewTime); }
        }

        async void GoNewTime()
        {
            NewTime = new NewTimeViewModel();
            await navigationService.Navigate("NewTimePage");
        }
        #endregion
    }
}
