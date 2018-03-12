using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BCL.Models;

namespace Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCustomer : ContentPage
	{
		public EditCustomer ()
		{
			InitializeComponent ();
            Title = "Enregistrement de client";
		}

        public EditCustomer(Customer customer)
        {
            InitializeComponent();
            Title = "Édition de client";
            if (customer.Id == 0)
                customer.Id = -1;
            FillingFields(customer);
        }

        protected void FillingFields(Customer Customer)
        {
            entryAddress.Text = Customer.Address;
            entryCity.Text = Customer.City;
            entryMail.Text = Customer.Mail;
            entryName.Text = Customer.Name;
            entryPacage.Text = Customer.Pacage;
            entryPostalCode.Text = Customer.PostalCode;
            labelHiddenId.Text = Customer.Id.ToString();
        }

        protected Customer CreateCustomerFromFields()
        {
            Customer customer = new Customer
            {
                Address = entryAddress.Text,
                City = entryCity.Text,
                Id = int.Parse(labelHiddenId.Text),
                LastUpdate = DateTime.Now,
                Mail = entryMail.Text,
                Name = entryName.Text,
                Pacage = entryPacage.Text,
                PostalCode = entryPostalCode.Text
            };
            if (string.IsNullOrWhiteSpace(customer.Mail))
                customer.Mail = Customer.DefaultMail;
            return customer;
        }

        protected void Save_Clicked(object sender, EventArgs e)
        {
            Customer customer = CreateCustomerFromFields();
            string validationErrors = customer.ValidationErrors() +
                App.LocalContext.ValidationErrors(customer);
            if (string.IsNullOrEmpty(validationErrors))
            {
                if (customer.Id != 0)
                    App.LocalContext.Customers.Remove(
                        App.LocalContext.Customers.Where(c => c.Id == customer.Id).First());
                
                App.LocalContext.Customers.Add(customer);

                App.LocalContext.Save(Enum.LocalDataType.Customers);

                App.Current.MainPage = new NavigationPage(new Menu());
            }
            else
                DisplayAlert("Erreur", validationErrors, "OK");
        }
    }
}