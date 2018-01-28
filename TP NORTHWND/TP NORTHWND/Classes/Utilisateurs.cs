using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_NORTHWND
{
    public partial class Utilisateurs
    {
        public Utilisateurs(string Identifiant, string MotDePasse)
        {
            this.Identifiant = Identifiant;
            this.MotDePasse = MotDePasse;
        }
    }
}
