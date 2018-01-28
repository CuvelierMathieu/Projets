using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TP_NORTHWND
{
    /// <summary>
    /// Logique d'interaction pour NouvelleCommande.xaml
    /// </summary>
    public partial class NouvelleCommande : Window
    {
        //Attributs qui devront être récupérés par la fenêtre mère
        public List<Order_Details> detail_commande = new List<Order_Details>();
        public Orders commande = new Orders();
        //Attributs dont on n'a pas besoin en dehors de la fenêtre
        private int Id_produit_selectionne;
        private int Id_produit_commande_selectionne;

        public NouvelleCommande(string Id_client)
        {
            //Ce constructeur impose d'avoir un client identifié au préalable
            commande.CustomerID = Id_client;
            InitializeComponent();
        }

        private void AjoutProduit(object sender, RoutedEventArgs e)
        {
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                //Identifier le produit sélectionné
                Products produit = contexte.Products.Where(a =>
                a.ProductID == this.Id_produit_selectionne).First();
                //Compter combien d'exemplaires de ce produit sont déjà commandés
                short quantite = 0;
                if (this.detail_commande.Where(a => a.ProductID == Id_produit_selectionne).
                    Count() != 0)
                {
                    quantite = this.detail_commande.Where(a =>
                    a.ProductID == Id_produit_selectionne).First().Quantity;
                }
                //Demander la nouvelle quantité
                Quantite fenetre_quantite = new Quantite(quantite);
                if (fenetre_quantite.ShowDialog() == true)
                {
                    if (quantite == 0)
                    {
                        if (fenetre_quantite.qte != 0)
                        {
                            //Dans le cas de l'ajout d'un nouveau produit à la commande
                            this.detail_commande.Add(new Order_Details(Id_produit_selectionne,
                                fenetre_quantite.qte, (decimal)produit.UnitPrice));
                        }
                    }
                    else
                    {
                        //Dans le cas d'une modification de quantite
                        if (fenetre_quantite.qte != 0)
                        {
                            this.detail_commande.Where(a => a.ProductID == Id_produit_selectionne).First().
                            Quantity = fenetre_quantite.qte;
                        }
                        else
                        {
                            detail_commande.Remove(detail_commande.Where(a =>
                            a.ProductID == Id_produit_selectionne).First());
                        }
                    }
                }
                MettreAJourAffichageCommande();
            }
        }

        private void MettreAJourAffichageCommande()
        {
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                List<string> liste = new List<string>();
                double cout_total = 0;
                foreach (Order_Details ligne in detail_commande)
                {
                    string nom_produit = contexte.Products.Where(a => a.ProductID == ligne.ProductID).
                        First().ProductName;
                    double cout_ligne = (double)ligne.UnitPrice * (double)ligne.Quantity;
                    cout_total += cout_ligne;
                    liste.Add("[" + ligne.ProductID.ToString() + "] " + nom_produit + " x" + 
                        ligne.Quantity.ToString() +" (Prix unitaire : " + 
                        ((double)ligne.UnitPrice).ToString() + "$ ; Sous-total : " + cout_ligne + "$)");
                }
                this.listboxCommande.ItemsSource = liste;
                this.labelPrix.Content = cout_total.ToString();
                if (this.detail_commande.Count() != 0) { this.buttonValiderCommande.IsEnabled = true; }
                else { this.buttonValiderCommande.IsEnabled = false; }
            }
        }

        private void ValiderCommande(object sender, RoutedEventArgs e)
        {
            if (ValidationChamps())
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    if (this.comboboxVendeurs.SelectedIndex != -1)
                    {
                        this.commande.EmployeeID = Convert.ToInt32(this.comboboxVendeurs.SelectedItem.ToString().
                            Split('[')[1].Replace("]", ""));
                    }
                    if (this.comboboxTransporteurs.SelectedIndex != -1)
                    {
                        this.commande.ShipVia = contexte.Shippers.Where(a =>
                        a.CompanyName == this.comboboxTransporteurs.SelectedItem.ToString()).First().ShipperID;
                    }
                    this.commande.OrderDate = this.datepickerDateCommande.SelectedDate;
                    this.commande.RequiredDate = this.datepickerDateLivraisonDemandee.SelectedDate;
                    if (this.checkboxDejaLivre.IsChecked == true)
                    { this.commande.ShippedDate = this.datepickerDateLivraisonEffectuee.SelectedDate; }
                    Customers client = contexte.Customers.Where(a => a.CustomerID == commande.CustomerID).First();
                    this.commande.ShipName = client.CompanyName;
                    this.commande.ShipAddress = client.Address;
                    this.commande.ShipCity = client.City;
                    this.commande.ShipRegion = client.Region;
                    this.commande.ShipPostalCode = client.PostalCode;
                    this.commande.ShipCountry = client.Country;
                }
                this.DialogResult = true;
            }
        }

        private bool ValidationChamps()
        {
            string erreurs = "";
            if (this.datepickerDateLivraisonDemandee.SelectedDate < this.datepickerDateCommande.SelectedDate)
            { erreurs += "\nLa date de livraison demandée est antérieure à celle de la commande"; }
            if (this.datepickerDateLivraisonEffectuee.SelectedDate < this.datepickerDateCommande.SelectedDate)
            { erreurs += "\nLa date de livraison effectuée est antérieure à celle de la commande"; }
            if (this.datepickerDateLivraisonEffectuee.SelectedDate > DateTime.Now
                && this.checkboxDejaLivre.IsChecked == true)
            { erreurs += "\nLa date de livraison effectuée est postérieure à celle d'aujourd'hui"; }

            string avertissements = "";
            if (this.comboboxTransporteurs.SelectedIndex == -1)
            { avertissements += "\nAucun transporteur n'a été sélectionné"; }
            if (this.comboboxVendeurs.SelectedIndex == -1)
            { avertissements += "\nAucun vendeur n'a été sélectionné"; }
            if (this.datepickerDateCommande.SelectedDate > DateTime.Now)
            { avertissements += "\nLa date de commande est post-datée"; }

            if (erreurs != "")
            {
                MessageBox.Show("La commande n'a pas pu être validée à cause des erreurs suivantes :\n" + erreurs,
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                if (avertissements != "")
                {
                    MessageBoxResult reponse = MessageBox.Show("Attention !\n" + avertissements + "\n\nOK pour ignorer ces"
                        + " avertissements, Annuler pour revenir à la commande", "Attention",
                        MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
                    if (reponse == MessageBoxResult.OK) { return true; }
                    else { return false; }
                }
                else { return true; }
            }
        }

        private void ModifierQuantite(object sender, RoutedEventArgs e)
        {
            short quantite_precedente = detail_commande.Where(a =>
            a.ProductID == Id_produit_commande_selectionne).First().Quantity;
            Quantite fenetre_quantite = new Quantite(quantite_precedente);
            if (fenetre_quantite.ShowDialog() == true)
            {
                if (fenetre_quantite.qte == 0)
                {
                    detail_commande.Remove(detail_commande.Where(a =>
                    a.ProductID == Id_produit_commande_selectionne).First());
                }
                else
                {
                    detail_commande.Where(a => a.ProductID == Id_produit_commande_selectionne).
                        First().Quantity = fenetre_quantite.qte;
                }
                MettreAJourAffichageCommande();
            }
        }

        private void listboxCommande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listboxCommande.SelectedIndex == -1)
            {
                this.buttonModifierQuantite.IsEnabled = false;
            }
            else
            {
                this.buttonModifierQuantite.IsEnabled = true;
                Id_produit_commande_selectionne = Convert.ToInt32(this.listboxCommande.SelectedItem.
                    ToString().Split(']')[0].Replace("[", ""));
            }
        }

        private void datagridProduits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.buttonAjouterProduit.IsEnabled = true;
            Id_produit_selectionne = Convert.ToInt32(this.datagridProduits.SelectedCells[0].
                Item.ToString().Split(',')[0].Split(' ')[3]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            //On charge les calendriers
            this.datepickerDateCommande.SelectedDate = DateTime.Now;
            this.datepickerDateLivraisonDemandee.SelectedDate = DateTime.Now.AddDays(1);
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                //On charge le nom du client
                this.labelClient.Content += contexte.Customers.Where(a => a.CustomerID == this.commande.CustomerID).First().CompanyName;
                //On charge la liste des vendeurs
                this.comboboxVendeurs.ItemsSource = contexte.Employees.Select(a => a.FirstName + " " + a.LastName + " [" + a.EmployeeID.ToString() + "]").ToList();
                //On charge la liste des transporteurs
                this.comboboxTransporteurs.ItemsSource = contexte.Shippers.Select(a => a.CompanyName).ToList();
                //On charge la liste des produits
                this.datagridProduits.ItemsSource =
                    (from produit in contexte.Products
                     select new
                     {
                         produit.ProductID,
                         produit.ProductName,
                         produit.QuantityPerUnit,
                         produit.UnitPrice
                     }).ToList();
                this.datagridProduits.Columns[0].Visibility = Visibility.Collapsed;
            }
        }

        private void BasculerAffichageDejaLivre(object sender, RoutedEventArgs e)
        {
            if (this.checkboxDejaLivre.IsChecked == true)
            {
                this.datepickerDateLivraisonEffectuee.Visibility = Visibility.Visible;
                this.datepickerDateLivraisonEffectuee.SelectedDate =
                    this.datepickerDateLivraisonDemandee.SelectedDate;
            }
            else
            {
                this.datepickerDateLivraisonEffectuee.Visibility = Visibility.Collapsed;
            }
        }
    }
}
