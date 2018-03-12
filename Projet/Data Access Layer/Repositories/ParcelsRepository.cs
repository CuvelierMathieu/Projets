using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;

namespace DAL
{
    public class ParcelsRepository : IRepository<Parcel>
    {
        #region Attributes
        public Context Context { get; set; }
        #endregion

        #region Constructors
        public ParcelsRepository()
        {
            Context = new Context();
        }

        public ParcelsRepository(Context context)
        {
            Context = context;
        }
        #endregion

        #region CRUD Methods
        public void Delete(int id)
        {
            Context.Parcels.Remove(Context.Parcels.Find(id));
        }

        public IEnumerable<Parcel> GetAll()
        {
            return Context.Parcels.Include("Contract");
        }

        public Parcel GetOneById(int id)
        {
            return Context.Parcels.Include("Contract").
                Where(p => p.Id == id).FirstOrDefault();
        }

        public void Register(Parcel item)
        {
            Context.Parcels.Add(item);
        }

        public void RegisterRange(IEnumerable<Parcel> listOfItems)
        {
            Context.Parcels.AddRange(listOfItems);
        }

        public void Update(int id, Parcel modifiedItem)
        {
            Parcel oldItem = GetOneById(id);
            oldItem.Contract = modifiedItem.Contract;
            oldItem.DeviceId = modifiedItem.DeviceId;
            oldItem.Name = modifiedItem.Name;
            oldItem.NumeroIlotPAC = modifiedItem.NumeroIlotPAC;
            oldItem.Surface = modifiedItem.Surface;
        }
        #endregion
        
        public void UploadToDataBase()
        {
            Context.SaveChanges();
        }
    }
}
