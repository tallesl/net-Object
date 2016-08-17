namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectLibrary.Tests.Data;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    [TestClass]
    public class AppSettingsTest
    {
        [TestMethod]
        public void Single()
        {
            // Arrange
            var expected = new SingleData { Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") };
            var appSettings = new NameValueCollection(ConfigurationManager.AppSettings);
            appSettings.Remove("Date");
            appSettings.Remove("Enum");
            appSettings.Remove("Text");
            appSettings.Remove("Array");

            // Act
            var actual = appSettings.ToObject<SingleData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        public void LengthyTest()
        {
            // Arrange
            var expected = new FlatData
            {
                Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                Date = new DateTime(1999, 1, 1),
                Enum = Enumeration.Two,
                Text = "level 1",
                Array = new[] { 1, 2, 3, },
            };

            // Act
            var actual = ConfigurationManager.AppSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Enum, actual.Enum);
            Assert.AreEqual(expected.Text, actual.Text);
            CollectionAssert.AreEqual(expected.Array, actual.Array);
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

            var expected = new FlatData
            {
                Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                Date = new DateTime(1999, 1, 1),
                Enum = Enumeration.Two,
                Text = "level 1",
            };

            // Act
            var actual = appSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Enum, actual.Enum);
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

            var expected = new FlatData
            {
                Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                Date = new DateTime(1999, 1, 1),
                Enum = Enumeration.Two,
            };

            // Act
            var actual = appSettings.ToObject<FlatData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Enum, actual.Enum);
            Assert.AreEqual(expected.Text, actual.Text);
        }
    }
}
