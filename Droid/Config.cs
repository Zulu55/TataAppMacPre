using System;
using SQLite.Net.Interop;
using TataAppMac.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TataAppMac.Droid.Config))]

namespace TataAppMac.Droid
{
    public class Config : IConfig
    {
		private string directoryDB;
		private ISQLitePlatform platform;

		public string DirectoryDB
		{
			get
			{
				if (string.IsNullOrEmpty(directoryDB))
				{
					directoryDB = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				}

				return directoryDB;
			}
		}

		public ISQLitePlatform Platform
		{
			get
			{
				if (platform == null)
				{
					platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
				}

				return platform;
			}
		}
	}
}