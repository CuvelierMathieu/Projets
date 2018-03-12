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
	public partial class ListCustomers : ContentPage
	{
		public ListCustomers ()
		{
			InitializeComponent ();
            Title = "Recherche de client";
            listViewCustomers.ItemsSource = App.LocalContext.Customers.OrderBy(a => a.Name);
		}

        protected void Customer_Tapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new CustomerDetails((BCL.Models.Customer)e.Item));
        }

        protected void NewCustomer_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditCustomer());
        }

        protected void EntryCustomerResearchField_TextChanged(object sender, EventArgs e)
        {
            string researchField = entryCustomerResearchField.Text.ToLower();
            if (string.IsNullOrWhiteSpace(researchField))
                listViewCustomers.ItemsSource = App.LocalContext.Customers.OrderBy(a => a.Name);
            else
                listViewCustomers.ItemsSource = App.LocalContext.Customers.
                    Where(c => c.Name.ToLower().Contains(researchField))
                    .OrderBy(a => a.Name);
        }
    }
}