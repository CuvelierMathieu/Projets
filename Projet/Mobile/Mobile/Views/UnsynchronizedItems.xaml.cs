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
	public partial class UnsynchronizedItems : ContentPage
	{
		public UnsynchronizedItems ()
		{
			InitializeComponent ();
            labelUnsyncedContracts.Text = App.LocalContext.UnsynchronizedContracts().Count().ToString();
            labelUnsyncedCustomers.Text = App.LocalContext.UnsynchronizedCustomers().Count().ToString();
            labelUnsyncedParcels.Text = App.LocalContext.UnsynchronizedParcels().Count().ToString();

            Title = "Éléments non synchronisés";
		}

        protected void Synchronizing_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Synchronizing());
            Navigation.RemovePage(this);
        }

    }
}