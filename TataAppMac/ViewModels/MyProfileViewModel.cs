namespace TataAppMac.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using TataAppMac.Models;
    using TataAppMac.Serviices;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

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
		ImageSource imageSource;
		MediaFile file;
        #endregion

        #region Properties
        public ObservableCollection<DocumentTypeItemViewModel> DocumentTypes
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
        #endregion

        #region Constructor
        public MyProfileViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            DocumentTypes = new ObservableCollection<DocumentTypeItemViewModel>();

			LoadEmployee();
			LoadDocumentTypes();

            IsEnabled = true;
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
				IsEnabled = false;
				await dialogService.ShowMessage("Error", checkConnetion.Message);
				return;
			}

			var urlAPI = Application.Current.Resources["URLAPI"].ToString();
			var mainViewModel = MainViewModel.GetInstance();
			var employee = mainViewModel.Employee;
			var response = await apiService.GetList<DocumentType>(
				urlAPI,
				"/api",
				"/DocumentTypes",
				employee.TokenType,
				employee.AccessToken);

			if (!response.IsSuccess)
			{
				IsRunning = false;
				IsEnabled = false;
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

            ReloadDocumentType((List<DocumentType>)response.Result);
            IsRunning = false;
			IsEnabled = true;
		}

        void ReloadDocumentType(List<DocumentType> documentTypes)
        {
            DocumentTypes.Clear();
            foreach (var documentType in documentTypes)
            {
                DocumentTypes.Add(new DocumentTypeItemViewModel
                {
                    Description = documentType.Description,
                    DocumentTypeId = documentType.DocumentTypeId,
                });
            }

			var mainViewModel = MainViewModel.GetInstance();
            DocumentTypeId = mainViewModel.Employee.DocumentTypeId;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentTypeId"));
		}

        void LoadEmployee()
        {
			var mainViewModel = MainViewModel.GetInstance();
			FirstName = mainViewModel.Employee.FirstName;
			LastName = mainViewModel.Employee.LastName;
			EmployeeCode = mainViewModel.Employee.EmployeeCode;
			DocumentTypeId = mainViewModel.Employee.DocumentTypeId;
			Document = mainViewModel.Employee.Document;
			Picture = mainViewModel.Employee.Picture;
			Email = mainViewModel.Employee.Email;
			Phone = mainViewModel.Employee.Phone;
			Address = mainViewModel.Employee.Address;
		}
		#endregion

		#region Commands
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
