namespace TataAppMac.ViewModels
{
    using System.ComponentModel;
    using TataAppMac.Models;
    using TataAppMac.Serviices;

    public class MyProfileViewModel : Employee, INotifyPropertyChanged
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
		public MyProfileViewModel()
		{
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();

            var mainViewModel = MainViewModel.GetInstance();
			FirstName = mainViewModel.Employee.FirstName;
			LastName = mainViewModel.Employee.LastName;
			EmployeeCode = mainViewModel.Employee.EmployeeCode;
			DocumentTypeId = mainViewModel.Employee.DocumentTypeId;

			IsEnabled = true;
		}
		#endregion
	}
}
