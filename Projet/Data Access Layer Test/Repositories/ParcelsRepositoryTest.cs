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
    public class ParcelsRepositoryTest
    {
        protected ParcelsRepository MockedRepository;

        [SetUp]
        public void Initialize()
        {
            MockedRepository = new ParcelsRepository();
        }

        [Test]
        [Category("ParcelsRepository")]
        public void RepositoryInitialized()
        {
            Assert.AreNotEqual(0, MockedRepository.Context.Parcels.CountAsync());
        }

        [Test]
        [Category("ParcelsRepository")]
        public void GetAllReturnsItems()
        {
            int unexpected = 0;

            IEnumerable<Parcel> list = MockedRepository.GetAll();
            int actual = 0;
            foreach (Parcel item in list)
                actual++;

            Assert.AreNotEqual(unexpected, actual);
        }

        [Test]
        [Category("ParcelsRepository")]
        public void GetOneReturnsOneItem()
        {
            Parcel actual = MockedRepository.GetOneById(1);

            Assert.IsNotNull(actual);
        }

        [Test]
        [Category("ParcelsRepository")]
        public void GetOneWithWrongIdReturnsNull()
        {
            Parcel actual = MockedRepository.GetOneById(-10);

            Assert.IsNull(actual);
        }

        [Test]
        [Category("ParcelsRepository")]
        public void RegisteringIsOperationnal()
        {
            Guid guid = Guid.NewGuid();
            Parcel expected = new Parcel
            {
                Name = guid.ToString(),
                Contract = new Contract { CreationDate = DateTime.Now }
            };

            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            Parcel actual = MockedRepository.GetAll().Where(p => p.Name == guid.ToString()).FirstOrDefault();

            Assert.AreSame(expected, actual);

            MockedRepository.Delete(actual.Id);
            MockedRepository.UploadToDataBase();
        }

        [Test]
        [Category("ParcelsRepository")]
        public void MultipleRegisteringIsOperationnal()
        {
            string guid = Guid.NewGuid().ToString();
            List<Parcel> expected = new List<Parcel>
            {
                new Parcel
                {
                    Name = guid,
                    Contract = new Contract { CreationDate = DateTime.Now }
                },
                new Parcel
                {
                    Name = guid,
                    Contract = new Contract { CreationDate = DateTime.Now }
                }
            };

            MockedRepository.RegisterRange(expected);
            MockedRepository.UploadToDataBase();

            List<Parcel> actual = MockedRepository.GetAll().Where(p => p.Name == guid).ToList();

            CollectionAssert.AreEquivalent(expected, actual);

            MockedRepository.Delete(actual[0].Id);
            MockedRepository.Delete(actual[1].Id);
            MockedRepository.UploadToDataBase();
        }

        [Test]
        [Category("ParcelsRepository")]
        public void UploadAnItemToDataBaseAllowsToRetrieveIt()
        {
            Guid guid = Guid.NewGuid();

            Parcel expected = new Parcel
            {
                DeviceId = guid
            };
            MockedRepository.Register(expected);
            MockedRepository.UploadToDataBase();

            ParcelsRepository otherRepository = new ParcelsRepository();
            IEnumerable<Parcel> allSavedParcels = otherRepository.GetAll();
            Parcel actual = new Parcel();
            foreach (Parcel item in allSavedParcels)
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
        [Category("ParcelsRepository")]
        public void RetrievedItemRefersCorrectlyToOtherObjectsThroughForeignKey()
        {
            Parcel parcel = MockedRepository.GetOneById(1);

            Assert.IsNotNull(parcel.Contract);
        }
        
        [Test]
        [Category("ParcelsRepository")]
        public void RetrievedItemsThroughGetAllCorrectlyReferToLinkedObjects()
        {
            List<Parcel> list = MockedRepository.GetAll().ToList();

            bool nullReferenceFound = false;
            for (int i = 0; i < 2; i++)
                if (list[i].Contract == null)
                {
                    nullReferenceFound = true;
                    break;
                }

            Assert.IsFalse(nullReferenceFound);
        }

        [Test]
        [Category("ParcelsRepository")]
        public void UpdateFonctionnal()
        {
            Parcel firstItem = MockedRepository.GetAll().First();
            int unexpected = firstItem.NumeroIlotPAC;

            firstItem.NumeroIlotPAC++;
            MockedRepository.Update(firstItem.Id, firstItem);
            MockedRepository.UploadToDataBase();

            int actual = MockedRepository.GetOneById(firstItem.Id).NumeroIlotPAC;

            Assert.AreNotEqual(unexpected, actual);
        }

    }
}
