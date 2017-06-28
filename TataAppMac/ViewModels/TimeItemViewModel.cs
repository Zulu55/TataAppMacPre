namespace TataAppMac.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TataAppMac.Models;
	using TataAppMac.Serviices;

	public class TimeItemViewModel : Time
    {
		#region Attributes
		NavigationService navigationService;
		#endregion

		#region Constructors
		public TimeItemViewModel()
		{
			navigationService = new NavigationService();
		}
		#endregion

		#region Commands
		public ICommand SelectTimeCommand
		{
			get { return new RelayCommand(SelectTime); }
		}

		async void SelectTime()
		{
			var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.TimeDetail = new TimeDetailViewModel(this);
			await navigationService.Navigate("TimeDetailPage");
		}
		#endregion
	}
}