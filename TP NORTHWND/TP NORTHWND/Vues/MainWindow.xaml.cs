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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TP_NORTHWND
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.textboxIdentifiant.Focus();
        }

        private void TentativeLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {   
                    string mdp_crypte = Cryptage.MD5Hash(this.passwordboxMotDePasse.Password);
                    //Ne jamais stocker de mot de passe en clair dans la mémoire de l'appli !
                    if (contexte.Utilisateurs.Where(a => a.Identifiant == textboxIdentifiant.Text
                    && a.MotDePasse == mdp_crypte).Count() == 1)
                    {
                        //Identification réussie
                        Navigation navigation = new Navigation(textboxIdentifiant.Text);
                        //On "nettoie" les champs login/password
                        textboxIdentifiant.Text = "";
                        passwordboxMotDePasse.Password = "";
                        if (!navigation.ShowDialog() == true) { this.Close(); }
                        //Si la fenêtre suivante ne s'est pas fermée "proprement" (ici, avec l'action de déconnexion),
                        //on ferme aussi cette fenêtre, ce qui va fermer l'appli en même temps
                    }
                    else
                    {
                        //Échec de l'authentification
                        MessageBox.Show("Échec lors de l'authentification !", "Erreur", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                this.textboxIdentifiant.Focus();
            }
        }

        private void NouvelUtilisateur(object sender, MouseButtonEventArgs e)
        {
            NouvelUtilisateur nouvelUtilisateur = new NouvelUtilisateur();
            if (nouvelUtilisateur.ShowDialog() == true)
            {
                using (NORTHWNDEntities contexte = new NORTHWNDEntities())
                {
                    contexte.Utilisateurs.Add(new Utilisateurs(nouvelUtilisateur.Identifiant,
                        nouvelUtilisateur.MotDePasseCrypte));
                    contexte.SaveChanges();
                    MessageBox.Show("L'utilisateur a été correctement enregistré", "Enregistrement",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.labelBonjour.Content += DateTime.Now.ToShortDateString();
        }
    }
}
