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
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        public ProductsRepository Repository = new ProductsRepository();

        [Route("")]
        public IEnumerable<Product> GetAll()
        {
            return Repository.GetAll();
        }

        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            Product product = Repository.GetOneById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [Route("add/{name}")]
        [HttpGet]
        public Product Add(string name)
        {
            Product item = new Product
            {
                Name = name,
                LastUpdate = DateTime.Now
            };

            if (item == null)
                throw new ArgumentNullException();
            Repository.Register(item);
            Repository.UploadToDataBase();
            return item;
        }

        [Route("update/{id}/{name}")]
        [HttpGet]
        public Product Update(int id, string name)
        {
            Product item = new Product
            {
                Id = id,
                Name = name,
                LastUpdate = DateTime.Now
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
