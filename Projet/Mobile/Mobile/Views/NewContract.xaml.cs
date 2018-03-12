using BCL.Models;
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
	public partial class NewContract : ContentPage
	{
        #region Attributes
        protected Contract Contract;
        protected List<Parcel> Parcels = new List<Parcel>();
        #endregion

        #region Constructor
        public NewContract(Customer customer)
        {
            InitializeComponent();
            listViewProductChoice.ItemsSource = App.LocalContext.Products.OrderBy(a => a.Name);
            Contract = new Contract
            {
                Customer = customer,
                CreationDate = DateTime.Now,
                DeviceId = App.AppInfos.DeviceId
            };
            Title = "Nouveau contrat";
        }
        #endregion
        
        #region Events

        #region Product Layout
        protected void Product_Tapped(object sender, ItemTappedEventArgs e)
        {
            Contract.Product = (Product)(e.Item);
            labelChoosenProduct.Text = String.Format("Produit choisi : {0}", Contract.Product.Name);
            ShowLayout(stackLayoutFields);
        }

        protected void EntryProductChoice_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryProductChoice.Text))
                listViewProductChoice.ItemsSource = App.LocalContext.Products.OrderBy(a => a.Name);
            else
                listViewProductChoice.ItemsSource = App.LocalContext.Products.
                    Where(p => p.Id.ToString().ToLower().Contains(entryProductChoice.Text.ToLower())
                    || p.Name.ToLower().Contains(entryProductChoice.Text.ToLower()))
                    .OrderBy(a => a.Name);
        }
        #endregion

        #region Fields Layout
        protected void ChoosenProduct_Tapped(object sender, EventArgs e)
        {
            Contract.Product = null;
            entryProductChoice.Text = "";
            ShowLayout(stackLayoutProductChoice);
        }

        protected void Parcellar_Toggled(object sender, ToggledEventArgs e)
        {
            Parcels = new List<Parcel>();
            ResetParcelFields();
            if (e.Value)
                ShowLayout(stackLayoutParcels);
        }
        
        protected void ContractOK_Clicked(object sender, EventArgs e)
        {
            ShowLayout(stackLayoutSignature);
        }
        #endregion

        #region Parcels Layout
        protected void ParcelsOK_Clicked(object sender, EventArgs e)
        {
            if (ValidateParcel())
                ShowLayout(stackLayoutFields);
        }

        protected void CancelParcel_Clicked(object sender, EventArgs e)
        {
            if (Parcels.Count() == 0)
                switchParcellar.IsToggled = false;
            ShowLayout(stackLayoutFields);
        }

        protected void NextParcel_Clicked(object sender, EventArgs e)
        {
            if (ValidateParcel())
                ResetParcelFields();
        }
        #endregion

        #region Signature Layout
        protected void SignatureOK_Clicked(object sender, EventArgs e)
        {
            if (ValidateContract())
                App.Current.MainPage = new NavigationPage(new Menu());
        }

        protected void SignatureCancel_Clicked(object sender, EventArgs e)
        {
            ShowLayout(stackLayoutFields);
        }
        #endregion

        #endregion

        #region Methods
        public void ShowLayout(StackLayout stackLayout)
        {
            stackLayoutFields.IsVisible = false;
            stackLayoutProductChoice.IsVisible = false;
            stackLayoutParcels.IsVisible = false;
            stackLayoutSignature.IsVisible = false;

            stackLayout.IsVisible = true;
        }

        protected bool ValidateParcel()
        {
            bool validated = true;
            string errorMessage = "";
            try
            {
                Parcel parcel = new Parcel
                {
                    Contract = this.Contract,
                    Name = entryParcelName.Text,
                    NumeroIlotPAC = int.Parse(entryParcelIlotPac.Text),
                    Surface = int.Parse(entryParcelSurface.Text),
                    DeviceId = App.AppInfos.DeviceId
                };
                errorMessage += parcel.ValidationErrorMessage();
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    validated = false;
                else
                    Parcels.Add(parcel);
            }
            catch
            {
                errorMessage += "Le numéro d'ilôt PAC et la surface doivent être des valeurs numériques\n";
                validated = false;
            }

            if (!validated)
                DisplayAlert("Erreur", errorMessage, "OK");

            return validated;
        }

        protected void ResetParcelFields()
        {
            entryParcelIlotPac.Text = string.Empty;
            entryParcelName.Text = string.Empty;
            entryParcelSurface.Text = string.Empty;
        }

        protected bool ValidateContract()
        {
            bool validated = true;
            string errorMessage = "";

            try
            {
                Contract.HarvestYear = int.Parse(entryHarvestYear.Text);
                if (Parcels.Count() != 0)
                    Contract.Surface = Parcels.Sum(p => p.Surface);
                else
                    Contract.Surface = 0;
                Contract.Parcellar = switchParcellar.IsToggled;
                Contract.Prime = int.Parse(entryPrime.Text);
                errorMessage += Contract.ValidationErrorMessage();
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    validated = false;
                else
                {
                    Contract.CustomerSignatureUri = Contract.DefaultSignatureUri;
                    Contract.UserSignatureUri = Contract.DefaultSignatureUri;
                    App.LocalContext.Contracts.Add(Contract);
                    App.LocalContext.Parcels.AddRange(Parcels);
                    App.LocalContext.Save(Enum.LocalDataType.Contracts);
                    App.LocalContext.Save(Enum.LocalDataType.Parcels);
                }
            }
            catch
            {
                errorMessage += "L'année de récolte et le montant de la prime doivent être " +
                    "des valeurs numériques\n";
                validated = false;
            }

            if (!validated)
                DisplayAlert("Erreur", errorMessage, "OK");

            return validated;
        }
        #endregion
    }
}