using BCL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/contracts")]
    public class ContractsController : ApiController
    {
        public ContractsRepository Repository = new ContractsRepository();

        [Route("")]
        public IEnumerable<Contract> GetAll()
        {
            return Repository.GetAll();
        }

        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            Contract contract = Repository.GetOneById(id);
            if (contract == null)
                return NotFound();
            return Ok(contract);
        }

        [Route("add/{deviceId}/{customerId}/{productId}/{harvestYear}/{surface}/{userSignatureUri}/{customerSignatureUri}/{parcellar}/{prime}/{creationdate}")]
        [HttpGet]
        public Contract Add(Guid deviceId, int customerId, int productId, int harvestYear, int surface, string userSignatureUri,
            string customerSignatureUri, bool parcellar, int prime, long creationdate)
        {
            CustomersRepository customersRepository = new CustomersRepository(Repository.Context);
            ProductsRepository productsRepository = new ProductsRepository(Repository.Context);
            Contract item = new Contract
            {
                DeviceId = deviceId,
                Customer = customersRepository.GetOneById(customerId),
                Product = productsRepository.GetOneById(productId),
                HarvestYear = harvestYear,
                Surface = surface,
                UserSignatureUri = userSignatureUri,
                CustomerSignatureUri = customerSignatureUri,
                CreationDate = new DateTime(creationdate),
                Parcellar = parcellar,
                Prime = prime
            };

            if (item == null)
                throw new ArgumentNullException();
            Repository.Register(item);
            Repository.UploadToDataBase();
            return item;
        }

        [Route("update/{id}/{deviceId}/{customerId}/{productId}/{harvestYear}/{surface}/{userSignatureUri}/{customerSignatureUri}/{parcellar}/{prime}/{creationdate}")]
        [HttpGet]
        public Contract Update(int id, Guid deviceId, int customerId, int productId, int harvestYear, int surface, string userSignatureUri,
            string customerSignatureUri, bool parcellar, int prime, DateTime creationdate)
        {
            CustomersRepository customersRepository = new CustomersRepository(Repository.Context);
            ProductsRepository productsRepository = new ProductsRepository(Repository.Context);
            Contract item = new Contract
            {
                Id = id,
                DeviceId = deviceId,
                Customer = customersRepository.GetOneById(customerId),
                Product = productsRepository.GetOneById(productId),
                HarvestYear = harvestYear,
                Surface = surface,
                UserSignatureUri = userSignatureUri,
                CustomerSignatureUri = customerSignatureUri,
                CreationDate = creationdate,
                Parcellar = parcellar,
                Prime = prime
            };

            if (item == null)
                throw new ArgumentNullException();
            if (Repository.GetOneById(item.Id) == null)
                throw new Exception();

            Repository.Update(item.Id, item);
            Repository.UploadToDataBase();
            return item;
        }
    }
}
