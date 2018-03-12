using BCL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mobile
{
    public class LocalContext
    {
        #region Attributes
        public List<Contract> Contracts { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Parcel> Parcels { get; set; }
        public List<Product> Products { get; set; }
        #endregion

        #region Constructors
        public LocalContext()
        {
            Contracts = JsonConvert.DeserializeObject<List<Contract>>(
                DependencyService.Get<ISaveAndLoad>().LoadText("contracts.txt"));
            Customers = JsonConvert.DeserializeObject<List<Customer>>(
                DependencyService.Get<ISaveAndLoad>().LoadText("customers.txt"));
            Parcels = JsonConvert.DeserializeObject<List<Parcel>>(
                DependencyService.Get<ISaveAndLoad>().LoadText("parcels.txt"));
            Products = JsonConvert.DeserializeObject<List<Product>>(
                DependencyService.Get<ISaveAndLoad>().LoadText("products.txt"));

            if (Contracts == null)
                Contracts = new List<Contract>();
            if (Customers == null)
                Customers = new List<Customer>();
            if (Parcels == null)
                Parcels = new List<Parcel>();
            if (Products == null)
                Products = new List<Product>();
        }
        #endregion

        #region Methods
        public void Save()
        {
            Save(Enum.LocalDataType.Contracts);
            Save(Enum.LocalDataType.Customers);
            Save(Enum.LocalDataType.Parcels);
            Save(Enum.LocalDataType.Products);
        }
        public void Save(Enum.LocalDataType localDataType)
        {
            switch (localDataType)
            {
                case Enum.LocalDataType.Contracts:
                    DependencyService.Get<ISaveAndLoad>().SaveText("contracts.txt",
                        JsonConvert.SerializeObject(Contracts));
                    break;
                case Enum.LocalDataType.Customers:
                    DependencyService.Get<ISaveAndLoad>().SaveText("customers.txt",
                        JsonConvert.SerializeObject(Customers));
                    break;
                case Enum.LocalDataType.Parcels:
                    DependencyService.Get<ISaveAndLoad>().SaveText("parcels.txt",
                        JsonConvert.SerializeObject(Parcels));
                    break;
                case Enum.LocalDataType.Products:
                    DependencyService.Get<ISaveAndLoad>().SaveText("products.txt",
                        JsonConvert.SerializeObject(Products));
                    break;
            }
        }

        public bool HasNonSynchronizedItems()
        {
            if (UnsynchronizedContracts().Count() +
                UnsynchronizedCustomers().Count() +
                UnsynchronizedParcels().Count()
                > 0)
                return true;
            return false;
        }

        public string ValidationErrors(Customer customer)
        {
            string message = "";

            if (customer.Mail != Customer.DefaultMail)
            {
                IEnumerable<Customer> customersWithTheSameMailAddress =
                   Customers.Where(c => c.Mail == customer.Mail);
                if (customersWithTheSameMailAddress.Count() > 1)
                {
                    message += "Adresse mail déjà utilisée\n";
                }
            }

            

            return message;
        }

        public List<Contract> UnsynchronizedContracts()
        {
            return Contracts.Where(a => a.Id == 0 || a.CreationDate > App.AppInfos.LastDownload).ToList();
        }
        public List<Customer> UnsynchronizedCustomers()
        {
            return Customers.Where(a => a.Id == 0 || a.LastUpdate > App.AppInfos.LastDownload).ToList();
        }
        public List<Parcel> UnsynchronizedParcels()
        {
            return Parcels.Where(a => a.Id == 0).ToList();
        }

        public void Reset()
        {
            Contracts = new List<Contract>();
            Customers = new List<Customer>();
            Parcels = new List<Parcel>();
            Products = new List<Product>();
        }
        #endregion
    }
}