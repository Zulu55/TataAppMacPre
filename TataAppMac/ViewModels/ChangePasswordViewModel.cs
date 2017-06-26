namespace TataAppMac.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TataAppMac.Models;
    using TataAppMac.Serviices;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		ApiService apiService;
		DialogService dialogService;
		NavigationService navigationService;
		DataService dataService;
		bool isRunning;
		bool isEnabled;
		#endregion

		#region Properties
		public bool IsRunning
		{
			set
			{
				if (isRunning != value)
				{
					isRunning = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
				}
			}
			get
			{
				return isRunning;
			}
		}

		public bool IsEnabled
		{
			set
			{
				if (isEnabled != value)
				{
					isEnabled = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
				}
			}
			get
			{
				return isEnabled;
			}
		}

		public string CurrentPassword 
        { 
            get;
            set; 
        }

		public string NewPassword 
        { 
            get; 
            set; 
        }

		public string ConfirmPassword 
        { 
            get; 
            set; 
        }
		#endregion

		#region Constructor
		public ChangePasswordViewModel()
		{
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
			dataService = new DataService();

			IsEnabled = true;
		}
		#endregion

		#region Commands
		public ICommand ChangePasswordCommand 
        { 
            get { return new RelayCommand(ChangePassword); } 
        }

		async void ChangePassword()
		{
			if (string.IsNullOrEmpty(CurrentPassword))
			{
				await dialogService.ShowMessage("Error", "You must enter the current password.");
				return;
			}

			var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

			if (employee.Password != CurrentPassword)
			{
				await dialogService.ShowMessage("Error", "The current password doesn´t  match.");
				return;
			}

			if (string.IsNullOrEmpty(NewPassword))
			{
				await dialogService.ShowMessage("Error", "You must enter a new password.");
				return;
			}

			if (NewPassword == CurrentPassword)
			{
				await dialogService.ShowMessage("Error", "The current password is equal to new password.");
				return;
			}

			if (NewPassword.Length < 6)
			{
				await dialogService.ShowMessage("Error", "The new password must have at least 6 characters.");
				return;
			}

			if (string.IsNullOrEmpty(ConfirmPassword))
			{
				await dialogService.ShowMessage("Error", "You must enter a password confirm.");
				return;
			}

			if (NewPassword != ConfirmPassword)
			{
				await dialogService.ShowMessage("Error", "The new password and confirm does not match.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();

			var request = new ChangePasswordRequest
			{
				CurrentPassword = CurrentPassword,
				Email = mainViewModel.Employee.Email,
				NewPassword = NewPassword,
			};

			var response = await apiService.ChangePassword(
                urlAPI, 
                "/api", 
                "/Employees/ChangePassword",
				employee.TokenType, 
                employee.AccessToken, 
                request);

			IsRunning = false;
			IsEnabled = true;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

            employee.Password = NewPassword;
            mainViewModel.Employee.Password = NewPassword;
            dataService.Update(employee);

			await dialogService.ShowMessage("Confirm", "Your password has been changed successfully.");
			await navigationService.Back();
		}
		#endregion
	}
}
