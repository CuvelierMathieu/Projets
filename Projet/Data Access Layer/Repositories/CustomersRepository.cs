using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;

namespace DAL
{
    public class CustomersRepository : IRepository<Customer>
    {
        #region Attributes
        public Context Context { get; set; }
        #endregion

        #region Constructors
        public CustomersRepository()
        {
            Context = new Context();
        }

        public CustomersRepository(Context context)
        {
            Context = context;
        }
        #endregion

        #region CRUD Methods
        public void Delete(int id)
        {
            Context.Customers.Remove(Context.Customers.Find(id));
        }

        public IEnumerable<Customer> GetAll()
        {
            return Context.Customers;
        }

        public Customer GetOneById(int id)
        {
            return Context.Customers.Find(id);
        }

        public void Register(Customer item)
        {
            Context.Customers.Add(item);
        }

        public void RegisterRange(IEnumerable<Customer> listOfItems)
        {
            Context.Customers.AddRange(listOfItems);
        }

        public void Update(int id, Customer modifiedItem)
        {
            Customer oldItem = GetOneById(id);
            oldItem.Address = modifiedItem.Address;
            oldItem.City = modifiedItem.City;
            oldItem.LastUpdate = DateTime.Now;
            oldItem.Mail = modifiedItem.Mail;
            oldItem.Name = modifiedItem.Name;
            oldItem.Pacage = modifiedItem.Pacage;
            oldItem.PostalCode = modifiedItem.PostalCode;
        }
        #endregion

        public void UploadToDataBase()
        {
            Context.SaveChanges();
        }
    }
}
