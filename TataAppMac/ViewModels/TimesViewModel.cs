using System.Collections.ObjectModel;
using TataAppMac.Serviices;
using TataAppMac.Models;
using Xamarin.Forms;
using System.Collections.Generic;

namespace TataAppMac.ViewModels
{
    public class TimesViewModel
    {
		#region Attributes
		private ApiService apiService;
		private DialogService dialogService;
		#endregion

		#region Properties
		public ObservableCollection<TimeItemViewModel> MyTimes
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public TimesViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();

            MyTimes = new ObservableCollection<TimeItemViewModel>();

            LoadTimes();
        }
        #endregion

        #region Methods
        private async void LoadTimes()
        {
			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;
            var response = await apiService.GetList<Time>(
                urlAPI,
                "/api",
                "/Times", 
                employee.TokenType,
                employee.AccessToken, 
                employee.EmployeeId);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var times = (List<Time>)response.Result;
            ReloadTimes(times);
        }

        private void ReloadTimes(List<Time> times)
        {
            MyTimes.Clear();
            foreach (var time in times)
            {
                MyTimes.Add(new TimeItemViewModel 
                {
                    Activity = time.Activity,    
                    ActivityId = time.ActivityId,
                    DateRegistered = time.DateRegistered,
                    DateReported = time.DateReported,
                    EmployeeId = time.EmployeeId,
                    From = time.From,
                    Project = time.Project,
                    ProjectId = time.ProjectId,
                    Remarks = time.Remarks,
                    TimeId = time.TimeId,
                    To = time.To,
                });
            }
        }
        #endregion
    }
}