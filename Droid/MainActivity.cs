using Android.App;
using Android.Content.PM;
using Android.OS;

namespace TataAppMac.Droid
{
    [Activity(Label = "TATA App", Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
		#region Singleton
		private static MainActivity instance;

		public static MainActivity GetInstance()
		{
			if (instance == null)
			{
				instance = new MainActivity();
			}

			return instance;
		}
		#endregion

		#region Methods
		protected override void OnCreate(Bundle bundle)
        {
			instance = this;
			
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);
			LoadApplication(new App());
        }
		#endregion
	}
}