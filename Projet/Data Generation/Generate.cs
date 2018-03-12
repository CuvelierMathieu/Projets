using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;

namespace Data_Generation
{
    public class Generate
    {
        public static void All()
        {
            All(new DAL.GlobalRepository());
        }

        public static void All(DAL.GlobalRepository globalRepository)
        {
            Guid guid = Guid.NewGuid();

            Customer customer1 = new Customer
            {
                Name = "Client test 1",
                Address = "Chaumière du bout du chemin",
                PostalCode = "49054",
                City = "Les Alleuds",
                Mail = "client1@uapl.fr",
                Pacage = "CUST1",
                LastUpdate = DateTime.Now
            };
            Customer customer2 = new Customer
            {
                Name = "Client test 2",
                Address = "Chaumière du bout du chemin",
                PostalCode = "52000",
                City = "Laval",
                Mail = "client2@uapl.fr",
                Pacage = "LAVAL",
                LastUpdate = DateTime.Now
            };

            Product mais = new Product
            {
                Name = "Maïs",
                LastUpdate = DateTime.Now
            };
            Product colza = new Product
            {
                Name = "Colza",
                LastUpdate = DateTime.Now
            };

            Contract contract1 = new Contract
            {
                DeviceId = guid,
                Customer = customer1,
                Product = mais,
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
            };
            Contract contract2 = new Contract
            {
                DeviceId = guid,
                Customer = customer1,
                Product = colza,
                HarvestYear = 2018,
                Surface = 15,
                CreationDate = DateTime.Now,
                Parcellar = true,
                Prime = 0
            };
            Contract contract3 = new Contract
            {
                DeviceId = guid,
                Customer = customer2,
                Product = mais,
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
            };

            Parcel parcel1 = new Parcel
            {
                Contract = contract2,
                DeviceId = guid,
                Name = "Parcelle 1",
                NumeroIlotPAC = 1,
                Surface = 5
            };
            Parcel parcel2 = new Parcel
            {
                Contract = contract2,
                DeviceId = guid,
                Name = "Parcelle 2",
                NumeroIlotPAC = 2,
                Surface = 10
            };

            globalRepository.CustomersRepository.RegisterRange(
                new List<Customer>
                {
                        customer1,
                        customer2
                });
            globalRepository.UploadToDataBase();

            globalRepository.ProductsRepository.RegisterRange(
                new List<Product>
                {
                        mais,
                        colza
                });
            globalRepository.UploadToDataBase();

            globalRepository.ContractsRepository.RegisterRange(
                new List<Contract>
                {
                        contract1,
                        contract2,
                        contract3
                });
            globalRepository.UploadToDataBase();

            globalRepository.ParcelsRepository.RegisterRange(
                new List<Parcel>
                {
                        parcel1,
                        parcel2
                });
            globalRepository.UploadToDataBase();
        }
    }
}
