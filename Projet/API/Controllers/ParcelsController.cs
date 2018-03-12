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
    [RoutePrefix("api/parcels")]
    public class ParcelsController : ApiController
    {
        public ParcelsRepository Repository = new ParcelsRepository();

        [Route("")]
        public IEnumerable<Parcel> GetAll()
        {
            return Repository.GetAll();
        }

        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            Parcel contract = Repository.GetOneById(id);
            if (contract == null)
                return NotFound();
            return Ok(contract);
        }

        [Route("add/{deviceId}/{contractid}/{name}/{numeroilotpac}/{surface}")]
        [HttpGet]
        public Parcel Add(Guid deviceId, int contractid, string name, int numeroilotpac, int surface)
        {
            ContractsRepository contractsRepository = new ContractsRepository(Repository.Context);
            Parcel item = new Parcel
            {
                DeviceId = deviceId,
                Surface = surface,
                Contract = contractsRepository.GetOneById(contractid),
                Name = name,
                NumeroIlotPAC = numeroilotpac
            };

            if (item == null)
                throw new ArgumentNullException();
            Repository.Register(item);
            Repository.UploadToDataBase();
            return item;
        }

        [Route("update/{id}/{deviceId}/{contractid}/{name}/{numeroilotpac}/{surface}")]
        [HttpGet]
        public Parcel Update(int id, Guid deviceId, int contractid, string name, int numeroilotpac, int surface)
        {
            ContractsRepository contractsRepository = new ContractsRepository(Repository.Context);
            Parcel item = new Parcel
            {
                Id = id,
                DeviceId = deviceId,
                Surface = surface,
                Contract = contractsRepository.GetOneById(contractid),
                Name = name,
                NumeroIlotPAC = numeroilotpac
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