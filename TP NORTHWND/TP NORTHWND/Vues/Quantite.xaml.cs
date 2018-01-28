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
    /// Logique d'interaction pour Quantite.xaml
    /// </summary>
    public partial class Quantite : Window
    {
        public short qte;

        public Quantite(short valeurBase)
        {
            InitializeComponent();
            qte = valeurBase;
            this.textboxValeur.Text = qte.ToString();
            this.textboxValeur.Focus();
            this.textboxValeur.SelectAll();
        }

        private void Validation()
        {
            try
            {
                qte = (short)(Convert.ToInt32(textboxValeur.Text));
                if (qte >= 0) { this.DialogResult = true; }
                else { MessageBox.Show("Ahem... Un nombre négatif, vraiment ?", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
            catch (Exception e) { MessageBox.Show(e.Message.ToString(), "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
            this.textboxValeur.Focus();
            this.textboxValeur.SelectAll();
        }

        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            Validation();
        }
    }
}
