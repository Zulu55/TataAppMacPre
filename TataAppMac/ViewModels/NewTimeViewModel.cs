using System;
using System.ComponentModel;
using TataAppMac.Models;
using TataAppMac.Serviices;

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
		}
		#endregion

		#region Methods

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
	}
}
