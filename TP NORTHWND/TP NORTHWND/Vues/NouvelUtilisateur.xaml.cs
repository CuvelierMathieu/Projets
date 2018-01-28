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
    /// Logique d'interaction pour NouvelUtilisateur.xaml
    /// </summary>
    public partial class NouvelUtilisateur : Window
    {
        public string Identifiant { get { return textboxIdentifiant.Text; } }
        public string MotDePasseCrypte { get { return Cryptage.MD5Hash(passwordboxMotDePasse.Password); } }

        public NouvelUtilisateur()
        {
            InitializeComponent();
        }

        private void Valider(object sender, RoutedEventArgs e)
        {
            if (textboxIdentifiant.Text != "" && passwordboxMotDePasse.Password != "")
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    if (contexte.Utilisateurs.Where(a => a.Identifiant == this.Identifiant).Count() != 0)
                    {
                        MessageBox.Show("Cet utilisateur existe déjà dans la base de données", "Erreur",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else { this.DialogResult = true; }
                }
            }
        }

        private void Annuler(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
