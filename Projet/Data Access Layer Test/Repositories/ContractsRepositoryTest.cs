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
    public class ContractsRepositoryTest
    {
        protected ContractsRepository MockedRepository;

        [SetUp]
        public void Initialize()
        {
            MockedRepository = new ContractsRepository();
        }
        
        [Test]
        [Category("ContractsRepository")]
        public void RepositoryInitialized()
        {
            Assert.AreNotEqual(0, MockedRepository.Context.Contracts.CountAsync());
        }

        [Test]
        [Category("ContractsRepository")]
        public void GetAllReturnsItems()
        {
            int unexpected = 0;

            int actual = MockedRepository.GetAll().Count();
            
            Assert.AreNotEqual(unexpected, actual);
        }

        [Test]
        [Category("ContractsRepository")]
        public void GetOneReturnsOneItem()
        {
            Contract actual = MockedRepository.GetOneById(1);

            Assert.IsNotNull(actual);
        }

        [Test]
        [Category("ContractsRepository")]
        public void GetOneWithWrongIdReturnsNull()
        {
            Contract actual = MockedRepository.GetOneById(-10);

            Assert.IsNull(actual);
        }

        [Test]
        [Category("ContractsRepository")]
        public void RegisteringIsOperationnal()
        {
            Guid guid = Guid.NewGuid();

            Contract expected = new Contract
            {
                DeviceId = guid,
                Customer = new Customer() { Name = "debuggingCustomer", LastUpdate = DateTime.Now },
                Product = new Product() { Name = "debuggingProduct", LastUpdate = DateTime.Now },
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
            };

            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            Contract actual = MockedRepository.GetAll().Where(c => c.DeviceId == guid).FirstOrDefault();

            Assert.AreSame(expected, actual);

            MockedRepository.Delete(actual.Id);
            MockedRepository.UploadToDataBase();
        }

        [Test]
        [Category("ContractsRepository")]
        public void MultipleRegisteringIsOperationnal()
        {
            Guid guid = Guid.NewGuid();

            List<Contract> expected = new List<Contract>
            {
                new Contract
                {
                DeviceId = guid,
                Customer = new Customer() { Name = "debuggingCustomer", LastUpdate = DateTime.Now },
                Product = new Product() { Name = "debuggingProduct", LastUpdate = DateTime.Now },
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
                },
                new Contract
                {
                DeviceId = guid,
                Customer = new Customer() { Name = "debuggingCustomer", LastUpdate = DateTime.Now },
                Product = new Product() { Name = "debuggingProduct", LastUpdate = DateTime.Now },
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
                }
            };

            MockedRepository.RegisterRange(expected);
            MockedRepository.UploadToDataBase();

            List<Contract> actual = MockedRepository.GetAll().Where(c => c.DeviceId == guid).ToList();

            CollectionAssert.AreEquivalent(expected, actual);

            MockedRepository.Delete(actual[0].Id);
            MockedRepository.Delete(actual[1].Id);
            MockedRepository.UploadToDataBase();
        }

        [Test]
        [Category("ContractsRepository")]
        public void UploadAnItemToDataBaseAllowsToRetrieveIt()
        {
            Guid guid = Guid.NewGuid();

            Contract expected = new Contract
            {
                DeviceId = guid,
                Customer = new Customer() { LastUpdate = DateTime.Now },
                Product = new Product() { LastUpdate = DateTime.Now },
                HarvestYear = 2018,
                Surface = 0,
                CreationDate = DateTime.Now,
                Parcellar = false,
                Prime = 0
            };
            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            ContractsRepository otherRepository = new ContractsRepository();
            IEnumerable<Contract> allSavedContracts = otherRepository.GetAll();
            Contract actual = new Contract();
            foreach (Contract item in allSavedContracts)
                if (item.DeviceId == guid)
                {
                    actual = item;
                    break;
                }

            Assert.AreEqual(expected.DeviceId, actual.DeviceId);

            otherRepository.Delete(actual.Id);
            otherRepository.UploadToDataBase();
        }

        [Test]
        [Category("ContractsRepository")]
        public void RetrievedItemRefersCorrectlyToOtherObjectsThroughForeignKey1()
        {
            Contract contract = MockedRepository.GetOneById(1);

            Assert.IsNotNull(contract.Customer);
        }

        [Test]
        [Category("ContractsRepository")]
        public void RetrievedItemRefersCorrectlyToOtherObjectsThroughForeignKey2()
        {
            Contract contract = MockedRepository.GetOneById(1);

            Assert.IsNotNull(contract.Product);
        }

        [Test]
        [Category("ContractsRepository")]
        public void RetrievedItemsThroughGetAllCorrectlyReferToLinkedObjects()
        {
            List<Contract> list = MockedRepository.GetAll().ToList();

            bool nullReferenceFound = false;
            for (int i = 0; i < 3; i++)
                if (list[i].Customer == null || list[i].Product == null)
                {
                    nullReferenceFound = true;
                    break;
                }

            Assert.IsFalse(nullReferenceFound);
        }

        [Test]
        [Category("ContractsRepository")]
        public void UpdateFonctionnal()
        {
            Contract firstContract = MockedRepository.GetAll().First();
            int unexpected = firstContract.HarvestYear;

            firstContract.HarvestYear--;
            MockedRepository.Update(firstContract.Id, firstContract);
            MockedRepository.UploadToDataBase();

            int actual = MockedRepository.GetOneById(firstContract.Id).HarvestYear;

            Assert.AreNotEqual(unexpected, actual);
        }
    }
}
