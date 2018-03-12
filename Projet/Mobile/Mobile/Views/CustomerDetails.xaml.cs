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
	public partial class CustomerDetails : ContentPage
	{
        public BCL.Models.Customer Customer;

		public CustomerDetails (BCL.Models.Customer customer)
		{
			InitializeComponent ();
            Customer = customer;
            InitializeFields();
        }

        protected void InitializeFields()
        {
            Title = Customer.Name;
            labelAddress.Text = Customer.Address;
            labelCity.Text = Customer.City;
            labelMail.Text = Customer.Mail;
            labelName.Text = Customer.Name;
            labelPacage.Text = Customer.Pacage;
            labelPostalCode.Text = Customer.PostalCode;
        }

        protected void EditCustomer_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditCustomer(Customer));
            Navigation.RemovePage(this);
        }

        protected void NewContract_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewContract(Customer));
        }
    }
}