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
	public partial class Menu : ContentPage
	{
		public Menu ()
		{
			InitializeComponent ();
            Title = "Menu";
		}

        protected void ListCustomers_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListCustomers());
        }

        protected void Synchronization_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Synchronizing());
        }

        protected void Deconnection_Tapped(object sender, EventArgs e)
        {

        }

        protected void WaitingForSynchronization_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UnsynchronizedItems());
        }

        protected void About_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new About());
        }

        protected void DeviceInfo_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DeviceInfos());
        }

    }
}