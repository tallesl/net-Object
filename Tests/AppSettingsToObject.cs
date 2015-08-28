namespace ToObject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using ToObject.Exceptions;
    using ToObject.Tests.Data;

    [TestClass]
    public class AppSettingsToObject
    {
        [TestMethod]
        public void LengthyTest()
        {
            // Arrange
            var expected =
                new FlatData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Text = "level 1",
                };

            // Act
            var actual = ConfigurationManager.AppSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingProperty()
        {
            // Arrange
            var appSettings = new NameValueCollection(ConfigurationManager.AppSettings);
            appSettings.Add("Extra property", "whatever");

            // Act
            var actual = appSettings.ToObject<FlatData>();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingPropertySafe()
        {
            // Arrange
            var appSettings = new NameValueCollection(ConfigurationManager.AppSettings);
            appSettings.Add("Extra property", "whatever");

            var expected =
                new FlatData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Text = "level 1",
                };

            // Act
            var actual = appSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ValueNotFoundException))]
        public void MissingValue()
        {
            // Arrange
            var appSettings = new NameValueCollection(ConfigurationManager.AppSettings);
            appSettings.Remove("Text");

            // Act
            var actual = appSettings.ToObject<FlatData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ValueNotFoundException))]
        public void MissingValueSafe()
        {
            // Arrange
            var appSettings = new NameValueCollection(ConfigurationManager.AppSettings);
            appSettings.Remove("Text");

            var expected =
                new FlatData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                };

            // Act
            var actual = appSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Text, actual.Text);
        }
    }
}
