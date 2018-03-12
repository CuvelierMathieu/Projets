using Moq;
using NUnit.Framework;
using System;
using DAL;
using BCL.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data_Access_Layer_Test
{
    [TestFixture]
    public class ProductsRepositoryTest
    {
        protected ProductsRepository MockedRepository;

        [SetUp]
        public void Initialize()
        {
            MockedRepository = new ProductsRepository();
        }

        [Test]
        [Category("ProductsRepository")]
        public void RepositoryInitialized()
        {
            Assert.AreNotEqual(0, MockedRepository.Context.Products.CountAsync());
        }

        [Test]
        [Category("ProductsRepository")]
        public void GetAllReturnsItems()
        {
            int unexpected = 0;

            IEnumerable<Product> list = MockedRepository.GetAll();
            int actual = 0;
            foreach (Product item in list)
                actual++;

            Assert.AreNotEqual(unexpected, actual);
        }

        [Test]
        [Category("ProductsRepository")]
        public void GetOneReturnsOneItem()
        {
            Product actual = MockedRepository.GetOneById(1);

            Assert.IsNotNull(actual);
        }

        [Test]
        [Category("ProductsRepository")]
        public void GetOneWithWrongIdReturnsNull()
        {
            Product actual = MockedRepository.GetOneById(-10);

            Assert.IsNull(actual);
        }

        [Test]
        [Category("ProductsRepository")]
        public void RegisteringIsOperationnal()
        {
            Product expected = new Product { Id = -1 };

            MockedRepository.Register(expected);

            Product actual = MockedRepository.GetOneById(-1);

            Assert.AreSame(expected, actual);

            MockedRepository.Delete(-1);
        }

        [Test]
        [Category("ProductsRepository")]
        public void MultipleRegisteringIsOperationnal()
        {
            List<Product> expected = new List<Product>
            {
                new Product { Id = -1 },
                new Product { Id = -2 }
            };

            MockedRepository.RegisterRange(expected);

            List<Product> actual = new List<Product>
            {
                MockedRepository.GetOneById(-1),
                MockedRepository.GetOneById(-2)
            };

            CollectionAssert.AreEquivalent(expected, actual);

            MockedRepository.Delete(-1);
            MockedRepository.Delete(-2);
        }

        [Test]
        [Category("ProductsRepository")]
        public void UploadAnItemToDataBaseAllowsToRetrieveIt()
        {
            Guid guid = Guid.NewGuid();
            string name = guid.ToString();

            Product expected = new Product
            {
                Name = name,
                LastUpdate = DateTime.Today
            };
            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            ProductsRepository otherRepository = new ProductsRepository();
            IEnumerable<Product> allSavedProducts = otherRepository.GetAll();
            Product actual = new Product();
            foreach (Product item in allSavedProducts)
                if (item.Name == name)
                {
                    actual = item;
                    break;
                }

            Assert.AreEqual(expected.Name, actual.Name);

            otherRepository.Delete(actual.Id);
            otherRepository.UploadToDataBase();
        }

        [Test]
        [Category("ProductsRepository")]
        public void UpdateFonctionnal()
        {
            Product firstItem = MockedRepository.GetAll().First();
            DateTime unexpected = firstItem.LastUpdate;

            firstItem.LastUpdate = firstItem.LastUpdate.AddSeconds(1);
            MockedRepository.Update(firstItem.Id, firstItem);
            MockedRepository.UploadToDataBase();

            DateTime actual = MockedRepository.GetOneById(firstItem.Id).LastUpdate;

            Assert.AreNotEqual(unexpected, actual);
        }

    }
}
