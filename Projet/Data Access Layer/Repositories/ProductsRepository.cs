using BCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductsRepository : IRepository<Product>
    {
        public Context Context { get; set; }

        public ProductsRepository()
        {
            Context = new Context();
        }

        public ProductsRepository(Context context)
        {
            Context = context;
        }

        public void Delete(int id)
        {
            Context.Products.Remove(Context.Products.Find(id));
        }

        public IEnumerable<Product> GetAll()
        {
            return Context.Products;
        }

        public Product GetOneById(int id)
        {
            return Context.Products.Find(id);
        }

        public void Register(Product item)
        {
            Context.Products.Add(item);
        }

        public void RegisterRange(IEnumerable<Product> listOfItems)
        {
            Context.Products.AddRange(listOfItems);
        }

        public void Update(int id, Product modifiedItem)
        {
            Product oldItem = GetOneById(id);
            oldItem.LastUpdate = DateTime.Now;
            oldItem.Name = modifiedItem.Name;
        }

        public void UploadToDataBase()
        {
            Context.SaveChanges();
        }
    }
}
