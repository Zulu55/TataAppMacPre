namespace TataAppMac.Views
{
    using System.Threading.Tasks;
    using TataAppMac.Serviices;
    using TataAppMac.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;

    public partial class LocationsPage : ContentPage
    {
        #region Attributes
        GeolocatorService geolocatorService;
        #endregion

        #region Constructors
        public LocationsPage()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();
            MoveToCurrentLocation();
        }
        #endregion

        #region Methods
        async void MoveToCurrentLocation()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 && geolocatorService.Longitude != 0)
            {
                var position = new Position(geolocatorService.Latitude, geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(.3)));
            }

			await ShowPins();
		}

        async Task ShowPins()
        {
            var locationsViewModel = LocationsViewModel.GetInstance();
            await locationsViewModel.LoadPins();
            foreach (var pin in locationsViewModel.Pins)
            {
                MyMap.Pins.Add(pin);
            }
        }
        #endregion
    }
}