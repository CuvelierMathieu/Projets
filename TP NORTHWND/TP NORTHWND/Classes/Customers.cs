using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_NORTHWND
{
    public partial class Customers
    {
        //Le constructeur qui servira de nouveau client par défaut lors de la création
        //du nouveau client
        public Customers(bool FromScratch)
        {
            if (FromScratch)
            {
                this.Address = "Avenue Lattre de Tassigny";
                this.City = "Angers";
                this.CompanyName = "IMIE - Angers";
                this.ContactName = "Anne Onyme";
                this.ContactTitle = "Directrice";
                this.Country = "France";
                this.CustomerID = "IMIEA";
                this.PostalCode = "49100";
            }
        }
    }
}
