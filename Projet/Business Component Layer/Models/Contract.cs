using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCL.Models
{
    public class Contract
    {
        public static string DefaultSignatureUri = "Non signé";

        public int Id { get; set; }
        public Guid DeviceId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int HarvestYear { get; set; }
        public int Surface { get; set; }
        public string UserSignatureUri { get; set; }
        public string CustomerSignatureUri { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Parcellar { get; set; }
        public int Prime { get; set; }

        public void Sign(Customer customer, string userSignatureUri, string customerSignatureUri)
        {
            throw new NotImplementedException();
        }

        public string ValidationErrorMessage()
        {
            string errorMessage = "";

            if (HarvestYear > DateTime.Now.Year)
                errorMessage += "L'année de récolte ne peut pas être supérieure à l'année en cours\n";
            if (Surface < 0)
                errorMessage += "La surface ne peut pas être négative\n";
            if (Prime < 0)
                errorMessage += "La prime ne peut pas être négative\n";

            return errorMessage;
        }
    }
}
