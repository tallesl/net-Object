namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectLibrary.Tests.Data;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class StringDictionaryTests
    {
        [TestMethod]
        public void Single()
        {
            // Arrange
            var dict = new Dictionary<string, string>() { { "Id", "366f4bd3-6717-4b14-9c79-70515296df7e" } };

            // Act
            var actual = dict.ToObject<SingleData>();

            // Assert
            Assert.AreEqual(new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"), actual.Id);
        }

        [TestMethod]
        public void LengthyTest()
        {
            var expected =
                new RecursiveData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Enum = Enumeration.One,
                    Text = "level 1",
                    Array = new[] { 1, 2, 3, },
                    Nested =
                        new RecursiveData
                        {
                            Id = new Guid("e591be31-289f-4a99-ba67-288ea24b7d7e"),
                            Date = new DateTime(1999, 2, 2),
                            Enum = Enumeration.Two,
                            Text = "level 2",
                            Array = new[] { 4, 5, 6, },
                            Nested =
                                new RecursiveData
                                {
                                    Id = null,
                                    Date = null,
                                    Enum = null,
                                    Text = null,
                                    Array = null,
                                    Nested =
                                        new RecursiveData
                                        {
                                            Id = new Guid("3bfdd62f-8b31-4aa2-931d-46535f291b0e"),
                                            Date = new DateTime(1999, 4, 4),
                                            Enum = Enumeration.Four,
                                            Text = "level 4",
                                            Array = new[] { 10, 11, 12, },
                                            Nested = null,
                                        },
                                },
                        },
                };

            var dict =
                new Dictionary<string, string>()
                {
                    // 1st level
                    { "Id", expected.Id.ToString() },
                    { "Date", expected.Date.ToString() },
                    { "Enum", expected.Enum.ToString() },
                    { "Text", expected.Text },
                    { "Array", string.Join(",", expected.Array) },

                    // 2nd level
                    { "Nested.Id", expected.Nested.Id.ToString() },
                    { "Nested.Date", expected.Nested.Date.ToString() },
                    { "Nested.Enum", expected.Nested.Enum.ToString()},
                    { "Nested.Text", expected.Nested.Text },
                    { "Nested.Array", string.Join(",", expected.Nested.Array) },

                    // 3rd level
                    { "Nested.Nested.Id", expected.Nested.Nested.Id.ToString() },
                    { "Nested.Nested.Date", expected.Nested.Nested.Date.ToString() },
                    { "Nested.Nested.Enum", expected.Nested.Nested.Enum.ToString() },
                    { "Nested.Nested.Text", expected.Nested.Nested.Text },
                    { "Nested.Nested.Array", null },

                    // 4th level
                    { "Nested.Nested.Nested.Id", expected.Nested.Nested.Nested.Id.ToString() },
                    { "Nested.Nested.Nested.Date", expected.Nested.Nested.Nested.Date.ToString() },
                    { "Nested.Nested.Nested.Enum", expected.Nested.Nested.Nested.Enum.ToString() },
                    { "Nested.Nested.Nested.Text", expected.Nested.Nested.Nested.Text },
                    { "Nested.Nested.Nested.Array", string.Join(",", expected.Nested.Nested.Nested.Array) },

                    // 5th level
                    { "Nested.Nested.Nested.Nested.Id", null },
                    { "Nested.Nested.Nested.Nested.Date", null },
                    { "Nested.Nested.Nested.Nested.Enum", null },
                    { "Nested.Nested.Nested.Nested.Text", null },
                    { "Nested.Nested.Nested.Nested.Array", null },
                };
            
            var actual = dict.ToObject<RecursiveData>();

            // 1st level
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Enum, actual.Enum);
            Assert.AreEqual(expected.Text, actual.Text);
            CollectionAssert.AreEqual(expected.Array, actual.Array);

            // 2nd level
            Assert.AreEqual(expected.Nested.Id, actual.Nested.Id);
            Assert.AreEqual(expected.Nested.Date, actual.Nested.Date);
            Assert.AreEqual(expected.Nested.Enum, actual.Nested.Enum);
            Assert.AreEqual(expected.Nested.Text, actual.Nested.Text);
            CollectionAssert.AreEqual(expected.Nested.Array, actual.Nested.Array);

            // 3rd level
            Assert.AreEqual(expected.Nested.Nested.Id, actual.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Date, actual.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Enum, actual.Nested.Nested.Enum);
            Assert.AreEqual(expected.Nested.Nested.Text, actual.Nested.Nested.Text);
            CollectionAssert.AreEqual(expected.Nested.Nested.Array, actual.Nested.Nested.Array);

            // 4th level
            Assert.AreEqual(expected.Nested.Nested.Nested.Id, actual.Nested.Nested.Nested.Id);
            Assert.AreEqual(expected.Nested.Nested.Nested.Date, actual.Nested.Nested.Nested.Date);
            Assert.AreEqual(expected.Nested.Nested.Nested.Enum, actual.Nested.Nested.Nested.Enum);
            Assert.AreEqual(expected.Nested.Nested.Nested.Text, actual.Nested.Nested.Nested.Text);
            CollectionAssert.AreEqual(expected.Nested.Nested.Nested.Array, actual.Nested.Nested.Nested.Array);

            // 5th level
            Assert.AreEqual(null, actual.Nested.Nested.Nested.Nested);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotParseException))]
        public void DifferentTypes()
        {
            // Arrange
            var dict =
                new Dictionary<string, string>()
                {
                    { "Id", "11" }, // Should be a Guid
                    { "Date", "1999-01-01" },
                    { "Enum", "One" },
                    { "Array", "1,2,3" },
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
                new Dictionary<string, string>()
                {
                    { "Id", "366f4bd3-6717-4b14-9c79-70515296df7e" },
                    { "Date", "1999-01-01" },
                    { "Enum", "One" },
                    { "Text", "eleven" },
                    { "Integer", "11" }, // There's no Integer
                };

            // Act
            dict.ToObject<RecursiveData>();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var dict =
                new Dictionary<string, string>()
                {
                    { "Id", "366f4bd3-6717-4b14-9c79-70515296df7e" },
                    { "Date", "1999-01-01" },
                    { "Enum", "One" },
                    { "Text", "eleven" },
                    { "Integer", "11" }, // There's no Integer
                };

            // Act
            dict.ToObjectSafe<RecursiveData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            // Arrange
            IDictionary<string, string> dict = null;

            // Act
            dict.ToObject<RecursiveData>();
        }
    }
}
