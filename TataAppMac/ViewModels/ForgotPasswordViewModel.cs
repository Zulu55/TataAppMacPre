namespace TataAppMac.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TataAppMac.Serviices;
    using Xamarin.Forms;

    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		ApiService apiService;
		DialogService dialogService;
		NavigationService navigationService;
		bool isRunning;
		bool isEnabled;
		#endregion

		#region Properties
		public string Email 
        { 
            get; 
            set; 
        }

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
		#endregion

		#region Constructor
		public ForgotPasswordViewModel()
		{
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

			IsEnabled = true;
		}
		#endregion

		#region Commands
		public ICommand SendNewPasswordCommand 
        { 
            get { return new RelayCommand(SendNewPassword); } 
        }

		private async void SendNewPassword()
		{
			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			if (string.IsNullOrEmpty(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter your email.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var response = await apiService.PasswordRecovery(
				urlAPI, 
                "/api", 
                "/Employees/PasswordRecovery", 
                Email);

			IsRunning = false;
			IsEnabled = true;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", "Your password can't be recovered.");
				return;
			}

			await dialogService.ShowMessage("Confirmation", "Your new password has been sent, check the new password in your email.");
			navigationService.SetMainPage("LoginPage");
		}

		public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }

		void Cancel()
		{
			navigationService.SetMainPage("LoginPage");
		}
		#endregion
	}
}