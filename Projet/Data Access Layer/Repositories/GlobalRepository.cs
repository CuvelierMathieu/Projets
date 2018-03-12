using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GlobalRepository
    {
        protected Context _context;
        public ContractsRepository ContractsRepository { get; set; }
        public CustomersRepository CustomersRepository { get; set; }
        public ParcelsRepository ParcelsRepository { get; set; }
        public ProductsRepository ProductsRepository { get; set; }

        public GlobalRepository()
        {
            _context = new Context();
            ContractsRepository = new ContractsRepository(_context);
            CustomersRepository = new CustomersRepository(_context);
            ParcelsRepository = new ParcelsRepository(_context);
            ProductsRepository = new ProductsRepository(_context);
        }

        public GlobalRepository(Context context)
        {
            this._context = context;
            ContractsRepository = new ContractsRepository(context);
            CustomersRepository = new CustomersRepository(context);
            ParcelsRepository = new ParcelsRepository(context);
            ProductsRepository = new ProductsRepository(context);
        }

        public void UploadToDataBase()
        {
            _context.SaveChanges();
        }
    }
}
