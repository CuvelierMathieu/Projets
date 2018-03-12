using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BCL.Models;

namespace Data_Generation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==== Génération des données ====");
            GlobalRepository globalRepository = new GlobalRepository();
            
            int nbCustomers = globalRepository.CustomersRepository.GetAll().Count();
            int nbContracts = globalRepository.ContractsRepository.GetAll().Count();
            int nbParcels = globalRepository.ParcelsRepository.GetAll().Count();
            int nbProducts = globalRepository.ProductsRepository.GetAll().Count();
            Console.WriteLine("Clients trouvés : {0}" +
                "\nContrats trouvés : {1}" +
                "\nParcelles trouvées : {2}" +
                "\nProduits trouvés : {3}",
                nbCustomers, nbContracts, nbParcels, nbProducts);
            if (nbCustomers == 0 && nbContracts == 0 && nbProducts == 0 && nbParcels == 0)
            {
                Console.WriteLine("Génération des données entamée");
                Generate.All(globalRepository);
                Console.WriteLine("Données par défaut initialisées");
            }
            else
                Console.WriteLine("Abandon de la génération des données");
            Console.ReadLine();
        }
    }
}
