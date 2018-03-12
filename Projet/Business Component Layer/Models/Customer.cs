using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCL.Models
{
    public class Customer
    {
        public static string DefaultMail = "Adresse mail non renseignée";

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Mail { get; set; }
        public string Pacage { get; set; }
        public DateTime LastUpdate { get; set; }
        
        #region Methods
        public string ValidationErrors()
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(Name))
                message += "Le nom n'est pas renseigné\n";
            if (string.IsNullOrWhiteSpace(Address))
                message += "L'adresse n'est pas renseignée\n";
            if (string.IsNullOrWhiteSpace(PostalCode))
                message += "Le code postal n'est pas renseigné\n";
            if (string.IsNullOrWhiteSpace(City))
                message += "La ville n'est pas renseignée\n";
            if (string.IsNullOrWhiteSpace(Pacage))
                message += "Le Pacage n'est pas renseigné\n";

            return message;
        }

        public string WarningErrors()
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(Mail))
                message += "L'adresse mail n'est pas renseignée\n";

            return message;
        }
        #endregion
    }
}
