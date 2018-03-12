using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        Context Context { get; set; }

        void UploadToDataBase();

        //CREATE
        void Register(T item);
        void RegisterRange(IEnumerable<T> listOfItems);

        //READ
        IEnumerable<T> GetAll();
        T GetOneById(int id);

        //UPDATE
        void Update(int id, T modifiedItem);

        //DELETE
        void Delete(int id);
    }
}
