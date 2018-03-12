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
    public class CustomersRepositoryTest
    {
        protected CustomersRepository MockedRepository;

        [SetUp]
        public void Initialize()
        {
            MockedRepository = new CustomersRepository();
        }

        [Test]
        [Category("CustomersRepository")]
        public void RepositoryInitialized()
        {
            Assert.AreNotEqual(0, MockedRepository.Context.Customers.CountAsync());
        }

        [Test]
        [Category("CustomersRepository")]
        public void GetAllReturnsItems()
        {
            int unexpected = 0;

            IEnumerable<Customer> list = MockedRepository.GetAll();
            int actual = 0;
            foreach (Customer item in list)
                actual++;

            Assert.AreNotEqual(unexpected, actual);
        }

        [Test]
        [Category("CustomersRepository")]
        public void GetOneReturnsOneItem()
        {
            Customer actual = MockedRepository.GetOneById(1);

            Assert.IsNotNull(actual);
        }

        [Test]
        [Category("CustomersRepository")]
        public void GetOneWithWrongIdReturnsNull()
        {
            Customer actual = MockedRepository.GetOneById(-10);

            Assert.IsNull(actual);
        }

        [Test]
        [Category("CustomersRepository")]
        public void RegisteringIsOperationnal()
        {
            Customer expected = new Customer { Id = -1 };

            MockedRepository.Register(expected);

            Customer actual = MockedRepository.GetOneById(-1);

            Assert.AreSame(expected, actual);

            MockedRepository.Delete(-1);
        }

        [Test]
        [Category("CustomersRepository")]
        public void MultipleRegisteringIsOperationnal()
        {
            List<Customer> expected = new List<Customer>
            {
                new Customer { Id = -1 },
                new Customer { Id = -2 }
            };

            MockedRepository.RegisterRange(expected);

            List<Customer> actual = new List<Customer>
            {
                MockedRepository.GetOneById(-1),
                MockedRepository.GetOneById(-2)
            };

            CollectionAssert.AreEquivalent(expected, actual);

            MockedRepository.Delete(-1);
            MockedRepository.Delete(-2);
        }

        [Test]
        [Category("CustomersRepository")]
        public void UploadAnItemToDataBaseAllowsToRetrieveIt()
        {
            Guid guid = Guid.NewGuid();
            string name = guid.ToString();

            Customer expected = new Customer
            {
                Name = name,
                LastUpdate = DateTime.Today
            };
            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            CustomersRepository otherRepository = new CustomersRepository();
            IEnumerable<Customer> allSavedCustomers = otherRepository.GetAll();
            Customer actual = new Customer();
            foreach (Customer item in allSavedCustomers)
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
        [Category("CustomersRepository")]
        public void UpdateFonctionnal()
        {
            Customer firstItem = MockedRepository.GetAll().First();
            int unexpected = int.Parse(firstItem.PostalCode);

            firstItem.PostalCode = (unexpected + 1).ToString();
            MockedRepository.Update(firstItem.Id, firstItem);
            MockedRepository.UploadToDataBase();

            int actual = int.Parse(MockedRepository.GetOneById(firstItem.Id).PostalCode);

            Assert.AreNotEqual(unexpected, actual);
        }

    }
}
