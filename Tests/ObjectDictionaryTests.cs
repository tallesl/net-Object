namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectLibrary.Tests.Data;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class ObjectDictionaryTests
    {
        [TestMethod]
        public void Single()
        {
            // Arrange
            var expected = new SingleData { Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") };
            var dict = new Dictionary<string, object>() { { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") }, };

            // Act
            var actual = dict.ToObject<SingleData>();

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        public void LengthyTest()
        {
            var expected = new RecursiveData
            {
                Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                Date = new DateTime(1999, 1, 1),
                Enum = Enumeration.One,
                Text = "level 1",
                Array = new[] { 1, 2, 3, },
                Nested = new RecursiveData
                {
                    Id = new Guid("e591be31-289f-4a99-ba67-288ea24b7d7e"),
                    Date = new DateTime(1999, 2, 2),
                    Enum = Enumeration.Two,
                    Text = "level 2",
                    Array = new[] { 4, 5, 6, },
                    Nested = new RecursiveData
                    {
                        Id = null,
                        Date = null,
                        Enum = null,
                        Text = null,
                        Nested = new RecursiveData
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
                new Dictionary<string, object>()
                {
                    // 1st level
                    { "Id", expected.Id },
                    { "Date", expected.Date },
                    { "Enum", expected.Enum },
                    { "Text", expected.Text },
                    { "Array", expected.Array },

                    // 2nd level
                    { "Nested.Id", expected.Nested.Id },
                    { "Nested.Date", expected.Nested.Date },
                    { "Nested.Enum", expected.Nested.Enum },
                    { "Nested.Text", expected.Nested.Text },
                    { "Nested.Array", expected.Nested.Array },

                    // 3rd level
                    { "Nested.Nested.Id", expected.Nested.Nested.Id },
                    { "Nested.Nested.Date", expected.Nested.Nested.Date },
                    { "Nested.Nested.Enum", expected.Nested.Nested.Enum },
                    { "Nested.Nested.Text", expected.Nested.Nested.Text },
                    { "Nested.Nested.Array", expected.Nested.Nested.Array },

                    // 4th level
                    { "Nested.Nested.Nested.Id", expected.Nested.Nested.Nested.Id },
                    { "Nested.Nested.Nested.Date", expected.Nested.Nested.Nested.Date },
                    { "Nested.Nested.Nested.Enum", expected.Nested.Nested.Nested.Enum },
                    { "Nested.Nested.Nested.Text", expected.Nested.Nested.Nested.Text },
                    { "Nested.Nested.Nested.Array", expected.Nested.Nested.Nested.Array },

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
        [ExpectedException(typeof(MismatchedTypesException))]
        public void DifferentTypes()
        {
            // Arrange
            var dict = new Dictionary<string, object>()
            {
                { "Id", 11 }, // Should be a Guid
                { "Date", new DateTime(1999, 1, 1) },
                { "One", Enumeration.One },
                { "Array", new[] { 1, 2, 3, } },
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
            var dict = new Dictionary<string, object>()
            {
                { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") },
                { "Date", new DateTime(1999, 1, 1) },
                { "One", Enumeration.One },
                { "Text", "eleven" },
                { "Array", new[] { 1, 2, 3, } },
                { "Integer", 11 }, // There's no Integer
            };

            // Act
            dict.ToObject<RecursiveData>();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var dict = new Dictionary<string, object>()
            {
                { "Id", new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") },
                { "Date", new DateTime(1999, 1, 1) },
                { "One", Enumeration.One },
                { "Text", "eleven" },
                { "Array", new[] { 1, 2, 3, } },
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
