namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectLibrary.Tests.Data;
    using System;
    using System.Data;
    using System.Linq;

    [TestClass]
    public class DataTableTests
    {
        [TestMethod]
        public void Single()
        {
            // Arrange
            var expected = new SingleData { Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e") };
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Rows.Add(new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"));

            // Act
            var actual = table.ToObject<SingleData>();

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(expected.Id, actual.Single().Id);
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
                        Array = null,
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

            var table = new DataTable();

            // 1st level
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Enum", typeof(int));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Array", typeof(int[]));

            // 2nd level
            table.Columns.Add("Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Enum", typeof(int));
            table.Columns.Add("Nested.Text", typeof(string));
            table.Columns.Add("Nested.Array", typeof(int[]));

            // 3rd level
            table.Columns.Add("Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Enum", typeof(int));
            table.Columns.Add("Nested.Nested.Text", typeof(string));
            table.Columns.Add("Nested.Nested.Array", typeof(int[]));

            // 4th level
            table.Columns.Add("Nested.Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Nested.Enum", typeof(int));
            table.Columns.Add("Nested.Nested.Nested.Text", typeof(string));
            table.Columns.Add("Nested.Nested.Nested.Array", typeof(int[]));

            // 5th level
            table.Columns.Add("Nested.Nested.Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Nested.Nested.Enum", typeof(int));
            table.Columns.Add("Nested.Nested.Nested.Nested.Text", typeof(string));
            table.Columns.Add("Nested.Nested.Nested.Nested.Array", typeof(int[]));

            table.Rows.Add(

                // 1st level
                expected.Id,
                expected.Date,
                expected.Enum,
                expected.Text,
                expected.Array,

                // 2nd level
                expected.Nested.Id,
                expected.Nested.Date,
                expected.Nested.Enum,
                expected.Nested.Text,
                expected.Nested.Array,

                // 3rd level
                expected.Nested.Nested.Id,
                expected.Nested.Nested.Date,
                expected.Nested.Nested.Enum,
                expected.Nested.Nested.Text,
                expected.Nested.Nested.Array,

                // 4th level
                expected.Nested.Nested.Nested.Id,
                expected.Nested.Nested.Nested.Date,
                expected.Nested.Nested.Nested.Enum,
                expected.Nested.Nested.Nested.Text,
                expected.Nested.Nested.Nested.Array,

                // 5th level
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value
            );

            var actual = table.ToObject<RecursiveData>().Single();

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
            var table = new DataTable();

            table.Columns.Add("Id", typeof(int)); // Should be a Guid
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Enum", typeof(int));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Array", typeof(int[]));

            table.Rows.Add(
                11,
                new DateTime(1999, 1, 1),
                Enumeration.One,
                "eleven",
                new[] { 1, 2, 3, }
            );

            // Act
            table.ToObject<RecursiveData>().Single();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingProperty()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Enum", typeof(int));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Array", typeof(int[]));
            table.Columns.Add("Integer", typeof(int)); // There's no Integer

            table.Rows.Add(
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                new DateTime(1999, 1, 1),
                Enumeration.One,
                "eleven",
                new[] { 1, 2, 3, },
                11
            );

            // Act
            table.ToObject<RecursiveData>().Single();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var table = new DataTable();

            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Enum", typeof(int));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Array", typeof(int[]));
            table.Columns.Add("Integer", typeof(int)); // There's no Integer

            table.Rows.Add(
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                new DateTime(1999, 1, 1),
                Enumeration.One,
                "eleven",
                new[] { 1, 2, 3, },
                11
            );

            // Act
            table.ToObjectSafe<RecursiveData>().Single();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullDataRow()
        {
            // Arrange
            DataRow row = null;

            // Act
            row.ToObject<RecursiveData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullDataTable()
        {
            // Arrange
            DataTable table = null;

            // Act
            table.ToObject<RecursiveData>().ToList(); // Forcing deferred execution
        }
    }
}
