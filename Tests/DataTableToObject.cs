namespace ToObject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Data;
    using System.Linq;
    using ToObject.Exceptions;
    using ToObject.Tests.Data;

    [TestClass]
    public class DataTableToObject
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

            var table = new DataTable();

            // 1st level
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Text", typeof(string));

            // 2nd level
            table.Columns.Add("Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Text", typeof(string));

            // 3rd level
            table.Columns.Add("Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Text", typeof(string));

            // 4th level
            table.Columns.Add("Nested.Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Nested.Text", typeof(string));

            // 5th level
            table.Columns.Add("Nested.Nested.Nested.Nested.Id", typeof(Guid));
            table.Columns.Add("Nested.Nested.Nested.Nested.Date", typeof(DateTime));
            table.Columns.Add("Nested.Nested.Nested.Nested.Text", typeof(string));

            table.Rows.Add(

                // 1st level
                expected.Id,
                expected.Date,
                expected.Text,

                // 2nd level
                expected.Nested.Id,
                expected.Nested.Date,
                expected.Nested.Text,

                // 3rd level
                expected.Nested.Nested.Id,
                expected.Nested.Nested.Date,
                expected.Nested.Nested.Text,

                // 4th level
                expected.Nested.Nested.Nested.Id,
                expected.Nested.Nested.Nested.Date,
                expected.Nested.Nested.Nested.Text,

                // 5th level
                DBNull.Value,
                DBNull.Value,
                DBNull.Value
            );

            var actual = table.ToObject<RecursiveData>().Single();

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
            var table = new DataTable();

            table.Columns.Add("Id", typeof(int)); // Should be a Guid
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Text", typeof(string));

            table.Rows.Add(
                11,
                new DateTime(1999, 1, 1),
                "eleven"
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
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Integer", typeof(int)); // There's no Integer

            table.Rows.Add(
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                new DateTime(1999, 1, 1),
                "eleven",
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
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Integer", typeof(int)); // There's no Integer

            table.Rows.Add(
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                new DateTime(1999, 1, 1),
                "eleven",
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
