namespace ToObject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using ToObject.Exceptions;
    using ToObject.Tests.Data;

    [TestClass]
    public class ObjectDictionaryToObject
    {
        [TestMethod]
        public void Single()
        {
            // Arrange
            var expected =
                new SingleData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e")
                };
            var dict =
                new Dictionary<string, object>()
                {
                    { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") },
                };

            // Act
            var actual = dict.ToObject<SingleData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        public void LengthyTest()
        {
            var expected =
                new RecursiveData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Text = "level 1",
                    Nested =
                        new RecursiveData
                        {
                            Id = new Guid("e591be31-289f-4a99-ba67-288ea24b7d7e"),
                            Date = new DateTime(1999, 2, 2),
                            Text = "level 2",
                            Nested =
                                new RecursiveData
                                {
                                    Id = null,
                                    Date = null,
                                    Text = null,
                                    Nested =
                                        new RecursiveData
                                        {
                                            Id = new Guid("3bfdd62f-8b31-4aa2-931d-46535f291b0e"),
                                            Date = new DateTime(1999, 4, 4),
                                            Text = "level 4",
                                            Nested = null,
                                        },
                                },
                        },
                };

            var dict =
                new Dictionary<string, object>()
                {
                    // 1st level
                    { "Id", expected.Id },
                    { "Date", expected.Date },
                    { "Text", expected.Text },

                    // 2nd level
                    { "Nested.Id", expected.Nested.Id },
                    { "Nested.Date", expected.Nested.Date },
                    { "Nested.Text", expected.Nested.Text },

                    // 3rd level
                    { "Nested.Nested.Id", expected.Nested.Nested.Id },
                    { "Nested.Nested.Date", expected.Nested.Nested.Date },
                    { "Nested.Nested.Text", expected.Nested.Nested.Text },

                    // 4th level
                    { "Nested.Nested.Nested.Id", expected.Nested.Nested.Nested.Id },
                    { "Nested.Nested.Nested.Date", expected.Nested.Nested.Nested.Date },
                    { "Nested.Nested.Nested.Text", expected.Nested.Nested.Nested.Text },

                    // 5th level
                    { "Nested.Nested.Nested.Nested.Id", null },
                    { "Nested.Nested.Nested.Nested.Date", null },
                    { "Nested.Nested.Nested.Nested.Text", null },
                };
            
            var actual = dict.ToObject<RecursiveData>();

            // 1st level
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Text, actual.Text);

            // 2nd level
            Assert.AreEqual(expected.Nested.Id, actual.Nested.Id);
            Assert.AreEqual(expected.Nested.Date, actual.Nested.Date);
            Assert.AreEqual(expected.Nested.Text, actual.Nested.Text);

            // 3rd level
            Assert.AreEqual(expected.Nested.Nested.Id, actual.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Date, actual.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Text, actual.Nested.Nested.Text);

            // 4th level
            Assert.AreEqual(expected.Nested.Nested.Nested.Id, actual.Nested.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Nested.Date, actual.Nested.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Nested.Text, actual.Nested.Nested.Nested.Text);

            // 5th level
            Assert.AreEqual(null, actual.Nested.Nested.Nested.Nested);
        }

        [TestMethod]
        [ExpectedException(typeof(MismatchedTypesException))]
        public void DifferentTypes()
        {
            // Arrange
            var dict =
                new Dictionary<string, object>()
                {
                    { "Id", 11 }, // Should be a Guid
                    { "Date", new DateTime(1999, 1, 1) },
                    { "Text", "eleven" },
                };

            // Act
            dict.ToObject<RecursiveData>();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingProperty()
        {
            // Arrange
            var dict =
                new Dictionary<string, object>()
                {
                    { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") },
                    { "Date", new DateTime(1999, 1, 1) },
                    { "Text", "eleven" },
                    { "Integer", 11 }, // There's no Integer
                };

            // Act
            dict.ToObject<RecursiveData>();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var dict =
                new Dictionary<string, object>()
                {
                    { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") },
                    { "Date", new DateTime(1999, 1, 1) },
                    { "Text", "eleven" },
                    { "Integer", 11 }, // There's no Integer
                };

            // Act
            dict.ToObjectSafe<RecursiveData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            // Arrange
            IDictionary<string, object> dict = null;

            // Act
            dict.ToObject<RecursiveData>();
        }
    }
}
