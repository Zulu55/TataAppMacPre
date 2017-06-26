namespace TataAppMac.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
	using TataAppMac.Helpers;
	using TataAppMac.Models;
    using TataAppMac.Serviices;
    using Xamarin.Forms;

    public class NewEmployeeViewModel : Employee, INotifyPropertyChanged
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
		ImageSource imageSource;
		MediaFile file;
		#endregion

		#region Properties
		public ObservableCollection<DocumentType> DocumentTypes 
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

		public ImageSource ImageSource
		{
			set
			{
				if (imageSource != value)
				{
					imageSource = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
				}
			}
			get
			{
				return imageSource;
			}
		}

		public string PasswordConfirm 
        { 
            get; 
            set; 
        }
		#endregion

		#region Constructor
		public NewEmployeeViewModel()
		{
			apiService = new ApiService();
			dialogService = new DialogService();
			navigationService = new NavigationService();
			dataService = new DataService();

			DocumentTypes = new ObservableCollection<DocumentType>();

            IsEnabled = true;

			LoadDocumentTypes();
		}
		#endregion

		#region Methods
		async void LoadDocumentTypes()
		{
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
			var response = await apiService.GetList<DocumentType>(
                urlAPI, 
                "/api", 
                "/DocumentTypes");

			IsRunning = false;
			IsEnabled = true;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

			ReloadDocumentTypes((List<DocumentType>)response.Result);
		}

        void ReloadDocumentTypes(List<DocumentType> documentTypes)
		{
			DocumentTypes.Clear();
			foreach (var documentType in documentTypes.OrderBy(dt => dt.Description))
			{
                DocumentTypes.Add(new DocumentType
                {
                    Description = documentType.Description,
                    DocumentTypeId = documentType.DocumentTypeId,
                });
			}
		}
		#endregion

		#region Commands
		public ICommand SaveCommand 
        { 
            get { return new RelayCommand(Save); } 
        }

		async void Save()
		{
			var checkConnetion = await apiService.CheckConnection();
			if (!checkConnetion.IsSuccess)
			{
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			if (string.IsNullOrEmpty(FirstName))
			{
				await dialogService.ShowMessage("Error", "You must enter a first name.");
				return;
			}

			if (string.IsNullOrEmpty(LastName))
			{
				await dialogService.ShowMessage("Error", "You must enter a last name.");
				return;
			}

			if (EmployeeCode == 0)
			{
				await dialogService.ShowMessage("Error", "You must enter an employee code.");
				return;
			}

			if (string.IsNullOrEmpty(Document))
			{
				await dialogService.ShowMessage("Error", "You must enter a document.");
				return;
			}

			if (string.IsNullOrEmpty(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter an email.");
				return;
			}

			if (!RegexUtilities.CheckEmail(Email))
			{
				await dialogService.ShowMessage("Error", "You must enter a valid email.");
				return;
			}

			if (string.IsNullOrEmpty(Phone))
			{
				await dialogService.ShowMessage("Error", "You must enter a phone.");
				return;
			}

			if (string.IsNullOrEmpty(Address))
			{
				await dialogService.ShowMessage("Error", "You must enter an address.");
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				await dialogService.ShowMessage("Error", "You must enter a password.");
				return;
			}

			if (Password.Length < 6)
			{
				await dialogService.ShowMessage("Error", "The password must have at least 6 characters.");
				return;
			}

			if (string.IsNullOrEmpty(PasswordConfirm))
			{
				await dialogService.ShowMessage("Error", "You must enter a password confirm.");
				return;
			}

			if (Password != PasswordConfirm)
			{
				await dialogService.ShowMessage("Error", "The password and confirm does not match.");
				return;
			}

			IsRunning = true;
			IsEnabled = false;

			var imageArray = FilesHelper.ReadFully(file.GetStream());
			file.Dispose();

			var employee = new Employee
			{
				Address = Address,
				Document = Document,
				DocumentTypeId = DocumentTypeId,
				Email = Email,
				EmployeeCode = EmployeeCode,
				EmployeeId = EmployeeId,
				FirstName = FirstName,
				ImageArray = imageArray,
				LastName = LastName,
				LoginTypeId = 1,
				Password = Password,
				Phone = Phone,
			};

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var response = await apiService.Post(
                urlAPI, 
                "/api", 
                "/Employees", 
                employee);

			IsRunning = false;
			IsEnabled = true;

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

			await dialogService.ShowMessage("Confirmation", "The employee was created, please login.");
			navigationService.SetMainPage("LoginPage");
		}

		public ICommand CancelCommand 
        {
            get { return new RelayCommand(Cancel); } 
        }

		void Cancel()
		{
			navigationService.SetMainPage("LoginPage");
		}

		public ICommand PickPictureCommand
		{
			get { return new RelayCommand(PickPicture); }
		}

		async void PickPicture()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await dialogService.ShowMessage(
					"Photo Not Supported",
					":( No available to pick photos from gallery.");
				return;
			}

			file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Small,
			});

			IsRunning = true;

			if (file != null)
			{
				ImageSource = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					return stream;
				});
			}

			IsRunning = false;
		}

		public ICommand TakePictureCommand
		{
			get { return new RelayCommand(TakePicture); }
		}

		async void TakePicture()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable ||
				!CrossMedia.Current.IsTakePhotoSupported)
			{
				await dialogService.ShowMessage(
					"No Camera",
					":( No camera available.");
				return;
			}

			file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg",
				PhotoSize = PhotoSize.Small,
			});

			IsRunning = true;

			if (file != null)
			{
				ImageSource = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					return stream;
				});
			}

			IsRunning = false;
		}
		#endregion
	}
}