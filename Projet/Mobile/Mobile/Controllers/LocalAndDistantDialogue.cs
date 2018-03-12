using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Mobile.Controllers
{
    public abstract class LocalAndDistantDialogue
    {
        protected static string basePath = "http://10.0.2.2:18/api/";

        #region Download methods
        public static void DownloadAllFromServerInto(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    DownloadContractsFromServerInto(localContext);
                    DownloadCustomersFromServerInto(localContext);
                    DownloadParcelsFromServerInto(localContext);
                    DownloadProductsFromServerInto(localContext);

                    App.AppInfos.LastDownload = DateTime.Now;
                    App.AppInfos.Save();
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
                
            }
        }

        public static void DownloadPartFromServerInto(LocalContext localContext, IEnumerable<Enum.LocalDataType> listOfDataTypes)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    foreach (Enum.LocalDataType dataType in listOfDataTypes)
                    {
                        switch (dataType)
                        {
                            case Enum.LocalDataType.Contracts:
                                DownloadContractsFromServerInto(localContext);
                                break;
                            case Enum.LocalDataType.Customers:
                                DownloadCustomersFromServerInto(localContext);
                                break;
                            case Enum.LocalDataType.Parcels:
                                DownloadParcelsFromServerInto(localContext);
                                break;
                            case Enum.LocalDataType.Products:
                                DownloadProductsFromServerInto(localContext);
                                break;
                        }
                    }
                    App.AppInfos.LastDownload = DateTime.Now;
                    App.AppInfos.Save();
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
                
            }
        }

        protected static void DownloadContractsFromServerInto(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basePath + "contracts");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    localContext.Contracts = JsonConvert.
                        DeserializeObject<List<Contract>>(reader.ReadToEnd());
                }
            }
        }

        protected static void DownloadCustomersFromServerInto(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basePath + "customers");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    localContext.Customers = JsonConvert.
                        DeserializeObject<List<Customer>>(reader.ReadToEnd());
                }
            }
        }

        protected static void DownloadParcelsFromServerInto(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basePath + "parcels");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    localContext.Parcels = JsonConvert.
                        DeserializeObject<List<Parcel>>(reader.ReadToEnd());
                }
            }
        }

        protected static void DownloadProductsFromServerInto(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(basePath + "products");

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    localContext.Products = JsonConvert.
                        DeserializeObject<List<Product>>(reader.ReadToEnd());
                }
            }
        }
        #endregion

        #region Upload methods
        public static void UploadToServerFrom(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    UploadCustomersIntoServerFrom(localContext);
                    UploadContractsIntoServerFrom(localContext);
                    UploadParcelsIntoServerFrom(localContext);

                    localContext.Contracts = new List<Contract>();
                    localContext.Parcels = new List<Parcel>();

                    App.AppInfos.LastUpload = DateTime.Now;
                    App.AppInfos.Save();
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
                
            }
        }

        protected static void UploadCustomersIntoServerFrom(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                IEnumerable<Customer> customersNeedingUpdate = localContext.Customers.
                Where(a => a.Id == 0 || a.LastUpdate > App.AppInfos.LastDownload);
                foreach (Customer item in customersNeedingUpdate)
                {
                    string path;
                    if (item.Id == 0)
                        path = String.Format("{0}customers/add/{1}/{2}/{3}/{4}/{5}/{6}/{7}",
                        basePath,
                        item.Name,
                        item.Address,
                        item.PostalCode,
                        item.City,
                        item.Mail,
                        item.Pacage,
                        item.LastUpdate.Ticks);
                    else
                        path = String.Format("{0}customers/update/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}",
                        basePath,
                        item.Id,
                        item.Name,
                        item.Address,
                        item.PostalCode,
                        item.City,
                        item.Mail,
                        item.Pacage,
                        item.LastUpdate.Ticks);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        item.Id = (JsonConvert.
                            DeserializeObject<Customer>(reader.ReadToEnd())).Id;
                    }
                }
            }
        }

        protected static void UploadContractsIntoServerFrom(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                IEnumerable<Contract> contractsWithNoId = localContext.Contracts.
                Where(a => a.Id == 0);
                foreach (Contract item in contractsWithNoId)
                {
                    string path = String.Format("{0}contracts/add/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                        basePath,
                        item.DeviceId,
                        item.Customer.Id,
                        item.Product.Id,
                        item.HarvestYear,
                        item.Surface,
                        item.UserSignatureUri,
                        item.CustomerSignatureUri,
                        item.Parcellar,
                        item.Prime,
                        item.CreationDate.Ticks);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        item.Id = (JsonConvert.
                            DeserializeObject<Contract>(reader.ReadToEnd())).Id;
                    }

                }
            }
        }

        protected static void UploadParcelsIntoServerFrom(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                IEnumerable<Parcel> parcelsWithNoId = localContext.Parcels.Where(a => a.Id == 0);
                foreach (Parcel item in parcelsWithNoId)
                {
                    string path = String.Format("{0}parcels/add/{1}/{2}/{3}/{4}/{5}",
                        basePath,
                        item.DeviceId,
                        item.Contract.Id,
                        item.Name,
                        item.NumeroIlotPAC,
                        item.Surface);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        item.Id = (JsonConvert.
                            DeserializeObject<Parcel>(reader.ReadToEnd())).Id;
                    }
                }
            }
        }
        #endregion

        #region Synchronization methods
        public static void SynchronizeServerAnd(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    UploadToServerFrom(localContext);
                    DownloadPartFromServerInto(
                    localContext,
                    new List<Enum.LocalDataType>
                    {
                    Enum.LocalDataType.Customers,
                    Enum.LocalDataType.Products
                    });
                    localContext.Save();
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
            }
        }

        public static void DownloadIfLastDownloadOlderThanOneDay(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    if (App.AppInfos.LastDownload.AddDays(1) <= DateTime.Now)
                    {
                        DownloadPartFromServerInto(localContext,
                            new List<Enum.LocalDataType>()
                            {
                        Enum.LocalDataType.Customers,
                        Enum.LocalDataType.Products
                            });
                        App.AppInfos.LastDownload = DateTime.Now;
                        App.AppInfos.Save();
                        localContext.Save();
                    }

                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
            }
        }

        public static void UploadIfLastUploadOlderThanOneDay(LocalContext localContext)
        {
            if (DeviceIsConnected())
            {
                try
                {
                    if (App.AppInfos.LastUpload.AddDays(1) <= DateTime.Now)
                    {
                        UploadToServerFrom(localContext);
                        App.AppInfos.LastUpload = DateTime.Now;
                        App.AppInfos.Save();

                    }
                }
                catch (Exception e)
                {
                    App.Current.MainPage.DisplayAlert("Erreur", "Une erreur est survenue lors de la communication avec le serveur :\n" +
                        e.Message, "OK");
                }
            }
        }
        #endregion
        
        protected static bool DeviceIsConnected()
        {
            bool connected = CrossConnectivity.Current.IsConnected;

            if (!connected)
                App.Current.MainPage.DisplayAlert("Erreur", "L'appareil n'a pas de connexion internet", "OK");

            return connected;
        }
    }
}
