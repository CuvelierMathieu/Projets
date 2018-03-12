using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;

namespace DAL
{
    public class ContractsRepository : IRepository<Contract>
    {
        #region Attributes
        public Context Context { get; set; }
        #endregion

        #region Constructors
        public ContractsRepository()
        {
            Context = new Context();
        }

        public ContractsRepository(Context context)
        {
            Context = context;
        }
        #endregion

        #region CRUD Methods
        public void Delete(int id)
        {
            Context.Contracts.Remove(Context.Contracts.Find(id));
        }

        public IEnumerable<Contract> GetAll()
        {
            return Context.Contracts.Include("Customer").Include("Product");
        }

        public Contract GetOneById(int id)
        {
            return Context.Contracts.Include("Customer").Include("Product").
                Where(c => c.Id == id).FirstOrDefault();
        }

        public void Register(Contract item)
        {
            Context.Contracts.Add(item);
        }

        public void RegisterRange(IEnumerable<Contract> listOfItems)
        {
            Context.Contracts.AddRange(listOfItems);
        }

        public void Update(int id, Contract modifiedItem)
        {
            Contract oldItem = GetOneById(id);
            oldItem.CreationDate = modifiedItem.CreationDate;
            oldItem.Customer = modifiedItem.Customer;
            oldItem.CustomerSignatureUri = modifiedItem.CustomerSignatureUri;
            oldItem.DeviceId = modifiedItem.DeviceId;
            oldItem.HarvestYear = modifiedItem.HarvestYear;
            oldItem.Parcellar = modifiedItem.Parcellar;
            oldItem.Prime = modifiedItem.Prime;
            oldItem.Product = modifiedItem.Product;
            oldItem.Surface = modifiedItem.Surface;
            oldItem.UserSignatureUri = modifiedItem.UserSignatureUri;
        }
        #endregion

        public void UploadToDataBase()
        {
            Context.SaveChanges();
        }
    }
}
