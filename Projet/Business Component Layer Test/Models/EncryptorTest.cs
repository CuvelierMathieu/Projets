using NUnit.Framework;
using System;

namespace UnitTestProject
{
    [TestFixture]
    public class EncryptorTest
    {
        protected BCL.Models.Encryptor Encryptor;

        [SetUp]
        public void Initialize()
        {
            Encryptor = new BCL.Models.Encryptor();
        }

        [Test]
        [TestCase("basicpassword")]
        [TestCase("eL4b0r/|7Ed*")]
        [Category("Encrypt")]
        public void EncryptedStringIsDifferentFromOriginalString(string original)
        {
            string actual = Encryptor.Encrypt(original);

            Assert.AreNotEqual(original, actual);
        }

        [Test]
        [Category("Encrypt")]
        public void NullStringReturnsException()
        {
            Assert.Throws<Exception>(() => Encryptor.Encrypt(""));
        }

        [Test]
        [Category("Encrypt")]
        public void StringWithSpacesReturnsException()
        {
            Assert.Throws<Exception>(() => Encryptor.Encrypt("password with spaces"));
        }

        [Test]
        [TestCase("basicpassword")]
        [TestCase("eL4b0r/|7Ed*")]
        [Category("Encrypt")]
        public void IdentiticalStringsGotSameEncryptedReturn(string input)
        {
            string original = input;
            string copy = input;

            string expected = Encryptor.Encrypt(original);
            string actual = Encryptor.Encrypt(copy);

            Assert.AreEqual(expected, actual);
        }
    }
}
