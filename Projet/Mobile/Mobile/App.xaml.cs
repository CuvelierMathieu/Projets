using Mobile.Context;
using Mobile.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mobile
{
	public partial class App : Application
	{
        public static LocalContext LocalContext;
        public static Models.AppInfos AppInfos;

		public App ()
		{
			InitializeComponent();

            if (!DataInitialisation.BaseFilesDetected())
                DataInitialisation.CreateBaseFiles();

            App.LocalContext = new LocalContext();
            App.AppInfos = Models.AppInfos.Load();

            LocalAndDistantDialogue.UploadIfLastUploadOlderThanOneDay(App.LocalContext);
            LocalAndDistantDialogue.DownloadIfLastDownloadOlderThanOneDay(App.LocalContext);

            Application.Current.MainPage = new NavigationPage(new Menu());
		}

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
