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
    /// Logique d'interaction pour CreationNouveauClient.xaml
    /// </summary>
    public partial class CreationNouveauClient : Window
    {
        public Customers client;

        public CreationNouveauClient()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new Customers(true);
            this.DataContext = client; //Passer le client en DataContext facilite le binding dans la partie XAML
        }

        private void Valider(object sender, RoutedEventArgs e)
        {
            using (NORTHWNDEntities contexte = new NORTHWNDEntities())
            {
                if (contexte.Customers.Where(a => a.CustomerID == client.CustomerID).Count() != 0)
                {
                    MessageBox.Show("Cet identifiant est déjà utilisé", "Erreur", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    if (this.client.CustomerID.Length != 5)
                    {
                        //Limite imposée par la base de données
                        MessageBox.Show("L'identifiant doit être de 5 caractères précisément", "Erreur", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else { this.DialogResult = true; }
                    //Cette commande va fermer la fenêtre et va renvoyer à la fenêtre-mère le résultat true
                }
            }
        }

        private void Annuler(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //Cette commande va fermer la fenêtre et va renvoyer à la fenêtre-mère le résultat false
        }
    }
}
