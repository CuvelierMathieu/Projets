using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCL.Models
{
    public class Parcel
    {
        public int Id { get; set; }
        public Contract Contract { get; set; }
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public int NumeroIlotPAC { get; set; }
        public int Surface { get; set; }

        public string ValidationErrorMessage()
        {
            string errorMessage = "";

            if (Surface < 0)
                errorMessage += "La surface ne peut pas être négative\n";
            if (NumeroIlotPAC < 0)
                errorMessage += "Le numéro d'ilôt PAC ne peut pas être négatif\n";
            if (string.IsNullOrWhiteSpace(Name))
                errorMessage += "Le nom de parcelle ne peut pas être vide\n";

            return errorMessage;
        }
    }
}
