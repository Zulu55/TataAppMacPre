namespace TataAppMac.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TataAppMac.Models;
    using TataAppMac.Serviices;

    public class EmployeeItemViewModel : Employee
    {
		#region Attributes
		NavigationService navigationService;
		#endregion

		#region Constructors
		public EmployeeItemViewModel()
        {
			navigationService = new NavigationService();
		}
		#endregion

		#region Commands
		public ICommand SelectEmployeeCommand
		{
			get { return new RelayCommand(SelectEmployee); }
		}

        async void SelectEmployee()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EmployeeDetail = new EmployeeDetailViewModel(this);
            await navigationService.Navigate("EmployeeDetailPage");
        }
        #endregion
    }
}
