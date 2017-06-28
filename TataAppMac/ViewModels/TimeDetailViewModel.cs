namespace TataAppMac.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TataAppMac.Models;
    using TataAppMac.Serviices;
    using Xamarin.Forms;

    public class TimeDetailViewModel : Time, INotifyPropertyChanged
    {
		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Attributes
		ApiService apiService;
		DialogService dialogService;
		NavigationService navigationService;
		DataService dataService;
		GeolocatorService geolocatorService;
		bool isRunning;
		bool isEnabled;
		Time time;
        int projectId;
        int activityId;
		public List<Project> projects;
		public List<Activity> activities;
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

		public int ProjectId
		{
			set
			{
				if (projectId != value)
				{
					projectId = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProjectId"));
				}
			}
			get
			{
				return projectId;
			}
		}

		public int ActivityId
		{
			set
			{
				if (activityId != value)
				{
					activityId = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActivityId"));
				}
			}
			get
			{
				return activityId;
			}
		}
		#endregion

		#region Constructor
		public TimeDetailViewModel(Time time)
        {
            this.time = time;

            TimeId = time.TimeId;
            EmployeeId = time.EmployeeId;
            DateRegistered = time.DateRegistered;
            DateReported = time.DateReported;
            From = time.From;
            To = time.To;
            Latitude = time.Latitude;
            Longitude = time.Longitude;
            Remarks = time.Remarks;

			FromString = string.Format("{0:HH:mm}", From);
			ToString = string.Format("{0:HH:mm}", To);

			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
			dataService = new DataService();
            geolocatorService = new GeolocatorService();

			IsEnabled = true;

			Projects = new ObservableCollection<ProjectItemViewModel>();
			Activities = new ObservableCollection<ActivityItemViewModel>();

			LoadPickers();
		}
		#endregion

		#region Singleton
		static TimeDetailViewModel instance;

		public static TimeDetailViewModel GetInstance()
		{
			return instance;
		}
		#endregion

		#region Methods
		async void LoadPickers()
		{
			IsEnabled = false;
			IsRunning = true;

			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var mainViewModel = MainViewModel.GetInstance();
			var employee = mainViewModel.Employee;

			var projectsResponse = await apiService.GetList<Project>(
				urlAPI,
				"/api",
				"/Projects",
				employee.TokenType,
				employee.AccessToken);

			if (projectsResponse.IsSuccess)
			{
				projects = (List<Project>)projectsResponse.Result;
				ReloadProjects();
			}

			var activitiesResponse = await apiService.GetList<Activity>(
				urlAPI,
				"/api",
				"/Activities",
				employee.TokenType,
				employee.AccessToken);

			if (activitiesResponse.IsSuccess)
			{
				activities = (List<Activity>)activitiesResponse.Result;
				ReloadActivities();
			}

			ProjectId = time.EmployeeId;
			ActivityId = time.ActivityId;

			IsEnabled = true;
			IsRunning = false;
		}

		public void ReloadProjects()
		{
			Projects.Clear();
			foreach (var project in projects.OrderBy(p => p.Description))
			{
				Projects.Add(new ProjectItemViewModel
				{
					Description = project.Description,
					ProjectId = project.ProjectId,
				});
			}
		}

		public void ReloadActivities()
		{
			Activities.Clear();
			foreach (var activity in activities.OrderBy(a => a.Description))
			{
				Activities.Add(new ActivityItemViewModel
				{
					Description = activity.Description,
					ActivityId = activity.ActivityId,
				});
			}
		}
		#endregion

		#region Commands
        public ICommand DeleteCommand
        {
			get { return new RelayCommand(Delete); }
		}

        async void Delete()
        {
            var answer = await dialogService.ShowConfirm("Confirm", "Are you sure to delete this record?");
            if (!answer)
            {
                return;
            }

			IsEnabled = false;
			IsRunning = true;

			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var mainViewModel = MainViewModel.GetInstance();
			var employee = mainViewModel.Employee;
			var timeToDelete = new Time2 { TimeId = time.TimeId, };
			var response = await apiService.Delete(
				urlAPI,
				"/api",
				"/Times",
				employee.TokenType,
				employee.AccessToken,
				timeToDelete);

			IsEnabled = true;
			IsRunning = false;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

			await navigationService.Back();
		}

        public ICommand SaveCommand
		{
			get { return new RelayCommand(Save); }
		}

		async void Save()
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

			IsEnabled = false;
			IsRunning = true;

			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = true;
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var mainViewModel = MainViewModel.GetInstance();
			var employee = mainViewModel.Employee;
			await geolocatorService.GetLocation();

            var timeToUpdate = new Time2
            {
                ActivityId = ActivityId,
                DateRegistered = time.DateRegistered,
                DateReported = DateReported,
                EmployeeId = time.EmployeeId,
                From = From,
                Latitude = geolocatorService.Latitude,
                Longitude = geolocatorService.Longitude,
                ProjectId = ProjectId,
                Remarks = Remarks,
                TimeId = time.TimeId,
                To = To,
            };

			var response = await apiService.Put(
				urlAPI,
				"/api",
				"/Times",
				employee.TokenType,
				employee.AccessToken,
				timeToUpdate);

			IsEnabled = true;
			IsRunning = false;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

			await navigationService.Back();
		}

		void ConvertHours()
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