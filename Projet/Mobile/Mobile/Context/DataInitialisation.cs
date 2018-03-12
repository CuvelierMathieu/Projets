using System;
using System.Collections.Generic;
using System.Text;
using BCL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Mobile.Context
{
    public abstract class DataInitialisation
    {
        public static void CreateTestDatas()
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

            List<Contract> contracts = new List<Contract>()
            {
                contract1,
                contract2,
                contract3
            };
            List<Customer> customers = new List<Customer>()
            {
                customer1,
                customer2
            };
            List<Parcel> parcels = new List<Parcel>()
            {
                parcel1,
                parcel2,
            };
            List<Product> products = new List<Product>()
            {
                mais,
                colza
            };

            string contractsJson = JsonConvert.SerializeObject(contracts);
            string customersJson = JsonConvert.SerializeObject(customers);
            string parcelsJson = JsonConvert.SerializeObject(parcels);
            string productsJson = JsonConvert.SerializeObject(products);

            DependencyService.Get<ISaveAndLoad>().SaveText("contracts.txt", contractsJson);
            DependencyService.Get<ISaveAndLoad>().SaveText("customers.txt", customersJson);
            DependencyService.Get<ISaveAndLoad>().SaveText("parcels.txt", parcelsJson);
            DependencyService.Get<ISaveAndLoad>().SaveText("products.txt", productsJson);
        }

        public static bool BaseFilesDetected()
        {
            try
            {
                LocalContext localContext = new LocalContext();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void CreateBaseFiles()
        {
            DependencyService.Get<ISaveAndLoad>().SaveText("contracts.txt", "");
            DependencyService.Get<ISaveAndLoad>().SaveText("customers.txt", "");
            DependencyService.Get<ISaveAndLoad>().SaveText("parcels.txt", "");
            DependencyService.Get<ISaveAndLoad>().SaveText("products.txt", "");
            DependencyService.Get<ISaveAndLoad>().SaveText("infos.txt", 
                JsonConvert.SerializeObject(new Models.AppInfos(true)));
        }
    }
}
