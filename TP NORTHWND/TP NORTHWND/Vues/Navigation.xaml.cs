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
using System.Windows.Threading;

namespace TP_NORTHWND
{
    /// <summary>
    /// Logique d'interaction pour Navigation.xaml
    /// </summary>
    public partial class Navigation : Window
    {
        private DispatcherTimer timer;
        private string IdentifiantUtilisateur;
        private Customers client = new Customers();
        private int Id_commande_selectionnee;

        public Navigation(string IdentifiantUtilisateur)
        {
            this.IdentifiantUtilisateur = IdentifiantUtilisateur;
            this.DataContext = client;
            InitializeComponent();
        }

        public Navigation()
        {
            Navigation fenetre_debug = new Navigation("Debug");
            fenetre_debug.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //On enclenche le timer pour l'horloge dans la status bar
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            //On génère la liste des clients
            GenererListeClients(this.comboboxListeClients);
            //On envoie le nom de l'utilisateur à la status bar
            this.menuitemConnecteEnTantQue.Header += IdentifiantUtilisateur;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.labelDate.Content = DateTime.Now.ToLongDateString() +
                " " + DateTime.Now.ToLongTimeString();
        }

        private void GenererListeClients(ComboBox listbox)
        {
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                List<string> liste_clients = contexte.Customers.Select(a => a.CustomerID + " - " +
                a.CompanyName).ToList();
                listbox.ItemsSource = liste_clients;
                liste_clients.Insert(0, "<Parcourir les commandes de>");
                listbox.SelectedIndex = 0;
            }
        }

        private void comboboxListeClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboboxListeClients.SelectedIndex == 0)
            {
                this.groupboxInfosClient.Visibility = Visibility.Collapsed;
                this.datagridCommandes.Visibility = Visibility.Collapsed;
                this.buttonNouvelleCommande.IsEnabled = false;
                this.buttonSupprimerClient.IsEnabled = false;
            }
            else
            {
                this.groupboxInfosClient.Visibility = Visibility.Visible;
                this.datagridCommandes.Visibility = Visibility.Visible;
                this.buttonNouvelleCommande.IsEnabled = true;
                this.buttonSupprimerClient.IsEnabled = true;
                ChargerListeCommandes();
            }
        }

        private void menuitemDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void NouveauClient(object sender, RoutedEventArgs e)
        {
            CreationNouveauClient creationNouveauClient = new CreationNouveauClient();
            if (creationNouveauClient.ShowDialog() == true)
            {
                //Enregistrement du nouveau client
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    contexte.Customers.Add(creationNouveauClient.client);
                    contexte.SaveChanges();
                }
                MessageBox.Show("Le client a bien été enregistré", "Validation", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                GenererListeClients(this.comboboxListeClients);
                this.comboboxListeClients.SelectedIndex = 0;
            }
        }

        private void NouvelleCommande(object sender, RoutedEventArgs e)
        {
            string Id_client = this.comboboxListeClients.SelectedValue.ToString().
                    Replace(" - ", ";").Split(';')[0];
            NouvelleCommande nouvelleCommande = new NouvelleCommande(Id_client);
            if (nouvelleCommande.ShowDialog() == true)
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    contexte.Orders.Add(nouvelleCommande.commande);
                    contexte.SaveChanges();
                    int id_commande = nouvelleCommande.commande.OrderID;
                    foreach (Order_Details ligne in nouvelleCommande.detail_commande)
                    {
                        ligne.OrderID = id_commande;
                        contexte.Order_Details.Add(ligne);
                    }
                    contexte.SaveChanges();
                    MessageBox.Show("La commande a bien été enregistrée sous le numéro " + id_commande.ToString(),
                       "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ChargerListeCommandes();
            }
        }

        private void SupprimerClient(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmation = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce client ?\n" +
                "Cette action est irréversible", "Attention !", MessageBoxButton.OKCancel,
                MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (confirmation == MessageBoxResult.OK)
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    //Récupération de l'Id client
                    string Id_client = this.comboboxListeClients.SelectedValue.ToString().
                    Replace(" - ", ";").Split(';')[0];
                    //Récupération du client
                    Customers client = contexte.Customers.Where(a => a.CustomerID == Id_client).First();
                    //Récupération de la liste des commandes du client
                    List<Orders> liste_commandes = contexte.Orders.Where(a =>
                    a.CustomerID == Id_client).ToList();
                    //Suppression des commandes du client
                    foreach (Orders commande in liste_commandes)
                    {
                        SupprimerCommande(commande);
                    }
                }
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    //Suppression du client
                    string Id_client = this.comboboxListeClients.SelectedValue.ToString().
                    Replace(" - ", ";").Split(';')[0];
                    Customers client = contexte.Customers.Where(a => a.CustomerID == Id_client).First();
                    contexte.Customers.Remove(client);
                    contexte.SaveChanges();
                }
                MessageBox.Show("Le client a bien été supprimé de la base de données", "Confirmation",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                GenererListeClients(this.comboboxListeClients);
            }
        }

        private void datagridCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.datagridCommandes.SelectedIndex != -1)
            {
                this.buttonSupprimerCommande.IsEnabled = true;
                string lecture_id = (this.datagridCommandes.SelectedCells[0].Item).ToString().Split(',')[0].
                    Split(' ')[3];
                Id_commande_selectionnee = Convert.ToInt32(lecture_id);
            }
            else
            {
                this.buttonSupprimerCommande.IsEnabled = false;
            }
        }

        private void buttonSupprimerCommande_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmation = MessageBox.Show("Êtes-vous sûr de vouloir supprimer " +
                "cette commande ?\nCette action est irréversible", "Attention !",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (confirmation == MessageBoxResult.OK)
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    Orders commande = contexte.Orders.Where(a => a.OrderID == Id_commande_selectionnee).First();
                    SupprimerCommande(commande);
                }
                MessageBox.Show("La commande " + Id_commande_selectionnee.ToString() + " a bien" +
                    " été supprimée", "Suppression", MessageBoxButton.OK, MessageBoxImage.Information);
                ChargerListeCommandes();
            }
        }

        private void SupprimerCommande(Orders commande)
        {
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                List<Order_Details> liste_temp = contexte.Order_Details.Where(a =>
                a.OrderID == commande.OrderID).ToList();
                foreach (Order_Details detail in liste_temp)
                {
                    contexte.Order_Details.Remove(detail);
                }
                Orders cmd = contexte.Orders.Where(a => a.OrderID == commande.OrderID).First();
                contexte.Orders.Remove(cmd);
                contexte.SaveChanges();
            }
        }

        private void ChargerListeCommandes()
        {
            try
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    string Id_Client = this.comboboxListeClients.SelectedItem.ToString().Replace(
                        " - ", ";").Split(';')[0].Replace(" ", "");
                    client = contexte.Customers.Where(a => a.CustomerID == Id_Client).First();
                    this.DataContext = client;
                    var query = from cmde in contexte.Orders
                                where cmde.CustomerID == Id_Client
                                join transporteur in contexte.Shippers
                                on cmde.ShipVia equals transporteur.ShipperID
                                into cmd_trsp
                                from trsp in cmd_trsp.DefaultIfEmpty()
                                join vendeur in contexte.Employees
                                on cmde.EmployeeID equals vendeur.EmployeeID
                                into cmd_vendeurs
                                from vendeur in cmd_vendeurs.DefaultIfEmpty()
                                select new
                                {
                                    NumeroCommande = cmde.OrderID,
                                    Vendeur = vendeur.FirstName + " " + vendeur.LastName,
                                    DateCommande = cmde.OrderDate,
                                    LivraisonDemandee = cmde.RequiredDate,
                                    LivraisonEffectuee = cmde.ShippedDate,
                                    Volume = cmde.Freight,
                                    Transporteur = trsp.CompanyName
                                };
                    this.datagridCommandes.ItemsSource = query.ToList();
                }
            }
            catch { }
        }
    }
}
