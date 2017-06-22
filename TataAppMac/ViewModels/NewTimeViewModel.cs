using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TataAppMac.Models;
using TataAppMac.Serviices;
using Xamarin.Forms;

namespace TataAppMac.ViewModels
{
    public class NewTimeViewModel : Time, INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		private ApiService apiService;
		private DialogService dialogService;
		private NavigationService navigationService;
		private DataService dataService;
		private bool isRunning;
		private bool isEnabled;
		private bool isRepeated;
		private bool isRepeatMonday;
		private bool isRepeatTuesday;
		private bool isRepeatWednesday;
		private bool isRepeatThursday;
		private bool isRepeatFriday;
		private bool isRepeatSaturday;
		private bool isRepeatSunday;
		#endregion

		#region Properties
		public ObservableCollection<ProjectItemViewModel> Projects
		{
			get;
			set;
		}

		public ObservableCollection<ActivityItemViewModel> Activities
		{
			get;
			set;
		}

		public string FromString
		{
			get;
			set;
		}

		public string ToString
		{
			get;
			set;
		}

		public DateTime Until
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

		public bool IsRepeated
		{
			set
			{
				if (isRepeated != value)
				{
					isRepeated = value;
                    if (!isRepeated)
                    {
                        TurnOffDays();
                    }
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeated"));
				}
			}
			get
			{
				return isRepeated;
			}
		}

        public bool IsRepeatMonday
		{
			set
			{
				if (isRepeatMonday != value)
				{
					isRepeatMonday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatMonday"));
				}
			}
			get
			{
				return isRepeatMonday;
			}
		}

		public bool IsRepeatTuesday
		{
			set
			{
				if (isRepeatTuesday != value)
				{
					isRepeatTuesday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatTuesday"));
				}
			}
			get
			{
				return isRepeatTuesday;
			}
		}

		public bool IsRepeatWednesday
		{
			set
			{
				if (isRepeatWednesday != value)
				{
					isRepeatWednesday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatWednesday"));
				}
			}
			get
			{
				return isRepeatWednesday;
			}
		}

		public bool IsRepeatThursday
		{
			set
			{
				if (isRepeatThursday != value)
				{
					isRepeatThursday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatThursday"));
				}
			}
			get
			{
				return isRepeatThursday;
			}
		}

		public bool IsRepeatFriday
		{
			set
			{
				if (isRepeatFriday != value)
				{
					isRepeatFriday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatFriday"));
				}
			}
			get
			{
				return isRepeatFriday;
			}
		}

		public bool IsRepeatSaturday
		{
			set
			{
				if (isRepeatSaturday != value)
				{
					isRepeatSaturday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatSaturday"));
				}
			}
			get
			{
				return isRepeatSaturday;
			}
		}

		public bool IsRepeatSunday
		{
			set
			{
				if (isRepeatSunday != value)
				{
					isRepeatSunday = value;
					if (value)
					{
						IsRepeated = true;
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatSunday"));
				}
			}
			get
			{
				return isRepeatSunday;
			}
		}
		#endregion

		#region Constructor
		public NewTimeViewModel()
        {
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
			dataService = new DataService();

            IsEnabled = true;
            Until = DateTime.Now;
            DateReported = DateTime.Now;

            Projects = new ObservableCollection<ProjectItemViewModel>();
            Activities = new ObservableCollection<ActivityItemViewModel>();

            LoadPickers();
		}
		#endregion

		#region Methods
		private async void LoadPickers()
        {
            IsEnabled = false;
            IsRunning = true;

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

			var projects = await apiService.GetList<Project>(
				urlAPI,
				"/api",
				"/Projects",
				employee.TokenType,
				employee.AccessToken);

            if (projects.IsSuccess)
            {
                ReloadProjects((List<Project>)projects.Result);
            }

			var activities = await apiService.GetList<Activity>(
            	urlAPI,
            	"/api",
            	"/Activities",
            	employee.TokenType,
            	employee.AccessToken);

			if (activities.IsSuccess)
			{
                ReloadActivities((List<Activity>)activities.Result);
			}

			IsEnabled = true;
            IsRunning = false;
		}

		private void ReloadProjects(List<Project> projects)
		{
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(new ProjectItemViewModel 
                {
                    Description = project.Description,
                    ProjectId = project.ProjectId,
                });
            }
        }

		private void ReloadActivities(List<Activity> activities)
		{
			Activities.Clear();
			foreach (var activity in activities)
			{
				Activities.Add(new ActivityItemViewModel
				{
					Description = activity.Description,
					ActivityId = activity.ActivityId,
				});
			}
		}

		private void TurnOffDays()
		{
			IsRepeatMonday = false;
            IsRepeatTuesday = false;
            IsRepeatWednesday = false;
            IsRepeatThursday = false;
            IsRepeatFriday = false;
            IsRepeatSaturday = false;
            IsRepeatSunday = false;
		}
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        private async void Save()
        {
			if (ProjectId == 0)
			{
				await dialogService.ShowMessage("Error", "You must select a project.");
				return;
			}

            if (ActivityId == 0)
			{
				await dialogService.ShowMessage("Error", "You must select an activity.");
				return;
			}

            ConvertHours();

			if (To <= From)
            {
				await dialogService.ShowMessage("Error", "The hour 'To' must be greather hour 'From'.");
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var mainViewModel = MainViewModel.GetInstance();
			var employee = mainViewModel.Employee;

			var newTimeRequest = new NewTimeRequest 
            {
                ActivityId = ActivityId,
                DateReported = DateReported,
                EmployeeId = employee.EmployeeId,
                From = From,
                IsRepeated = IsRepeated,
                IsRepeatFriday = IsRepeatFriday,
                IsRepeatMonday = IsRepeatMonday,
                IsRepeatSaturday = IsRepeatSaturday,
                IsRepeatSunday = IsRepeatSunday,
                IsRepeatThursday = IsRepeatThursday,
                IsRepeatTuesday = IsRepeatTuesday,
                IsRepeatWednesday = IsRepeatWednesday,
                ProjectId = ProjectId,
                Remarks = Remarks,
                To = To,
                Until = Until,
            };

			var response = await apiService.Post(
				urlAPI,
				"/api",
				"/Times",
				employee.TokenType,
				employee.AccessToken,
                newTimeRequest);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationService.Back();
		}

        private void ConvertHours()
        {
			int posTo = ToString.IndexOf(':');
			int posFrom = FromString.IndexOf(':');
            int toHour = 0, toMinute = 0, fromHour = 0, fromMinute = 0;

			if (posTo == -1)
			{
				int.TryParse(ToString, out toHour);
			}
			else
			{
				int.TryParse(ToString.Substring(0, posTo), out toHour);
				int.TryParse(ToString.Substring(posTo + 1), out toMinute);
			}

			if (posFrom == -1)
			{
				int.TryParse(FromString, out fromHour);
			}
			else
			{
				int.TryParse(FromString.Substring(0, posFrom), out fromHour);
				int.TryParse(FromString.Substring(posFrom + 1), out fromMinute);
			}

			To = new DateTime(1900, 1, 1, toHour, toMinute, 0);
			From = new DateTime(1900, 1, 1, fromHour, fromMinute, 0);
		}
        #endregion
    }
}