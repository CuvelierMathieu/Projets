using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Synchronizing : ContentPage
	{
		public Synchronizing ()
		{
			InitializeComponent ();
            InitializeFields();
            Title = "Synchronisation";
		}

        protected void InitializeFields()
        {
            ResetFields();
            labelLastDownload.Text += App.AppInfos.LastDownload.ToShortDateString()
                + " à " + App.AppInfos.LastDownload.ToShortTimeString();
            labelLastUpload.Text += App.AppInfos.LastUpload.ToShortDateString()
                + " à " + App.AppInfos.LastUpload.ToShortTimeString();
        }

        protected void ResetFields()
        {
            labelLastDownload.Text = "Dernière réception depuis le serveur : ";
            labelLastUpload.Text = "Dernier envoi vers le serveur : ";
        }

        protected void ForceSync_Clicked(object sender, EventArgs e)
        {
            Controllers.LocalAndDistantDialogue.SynchronizeServerAnd(App.LocalContext);
            InitializeFields();
        }

        protected void ForceReset_Clicked(object sender, EventArgs e)
        {
            App.LocalContext.Reset();

            Controllers.LocalAndDistantDialogue.DownloadPartFromServerInto(
                App.LocalContext,
                new List<Enum.LocalDataType>
                {
                    Enum.LocalDataType.Customers,
                    Enum.LocalDataType.Products
                });

            InitializeFields();

            App.LocalContext.Save();
        }
    }
}