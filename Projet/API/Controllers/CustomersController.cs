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
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        public CustomersRepository Repository = new CustomersRepository();

        [Route("")]
        public IEnumerable<Customer> GetAll()
        {
            return Repository.GetAll();
        }

        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            Customer customer = Repository.GetOneById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [Route("add/{name}/{address}/{postalcode}/{city}/{mail}/{pacage}/{lastupdate}")]
        [HttpGet]
        public Customer Add(string name, string address, string postalcode,
            string city, string mail, string pacage, long lastupdate)
        {
            Customer item = new Customer
            {
                Name = name,
                Address = address,
                PostalCode = postalcode,
                City = city,
                Mail = mail,
                Pacage = pacage,
                LastUpdate = new DateTime(lastupdate)
            };

            if (item == null)
                throw new ArgumentNullException();
            Repository.Register(item);
            Repository.UploadToDataBase();
            return item;
        }

        [Route("update/{id}/{name}/{address}/{postalcode}/{city}/{mail}/{pacage}/{lastupdate}")]
        [HttpGet]
        public Customer Update(int id, string name, string address, string postalcode,
            string city, string mail, string pacage, long lastupdate)
        {
            Customer item = new Customer
            {
                Id = id,
                Name = name,
                Address = address,
                PostalCode = postalcode,
                City = city,
                Mail = mail,
                Pacage = pacage,
                LastUpdate = new DateTime(lastupdate)
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
