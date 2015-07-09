namespace DataTableToObject.Tests
{
    using DataTableToObject.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Data;
    using System.Linq;

    public class SomeData
    {
        public int Integer { get; set; }

        public double FloatingPoint { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public Guid Id { get; set; }
    }

    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TypicalUsage()
        {
            // Arrange
            var expected =
                new SomeData
                {
                    Integer = 23,
                    FloatingPoint = 2.3,
                    Text = "twenty three",
                    Date = new DateTime(1999, 2, 3),
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e")
                };

            var table = new DataTable();
            table.Columns.Add("Integer", typeof(int));
            table.Columns.Add("FloatingPoint", typeof(double));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Id", typeof(Guid));
            table.Rows.Add(expected.Integer, expected.FloatingPoint, expected.Text, expected.Date, expected.Id);

            // Act
            var actual = table.ToObject<SomeData>().Single();

            // Assert
            Assert.AreEqual(expected.Integer, actual.Integer);
            Assert.AreEqual(expected.FloatingPoint, actual.FloatingPoint);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Id, actual.Id);
        }
        [TestMethod]
        [ExpectedException(typeof(MismatchedTypesException))]
        public void DifferentTypes()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Integer", typeof(int));
            table.Columns.Add("FloatingPoint", typeof(string)); // <-- not a string!
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Id", typeof(Guid));
            table.Rows.Add(23, "I'm not a float :)", "twenty three", new DateTime(1999, 2, 3),
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"));

            // Act
            var actual = table.ToObject<SomeData>().Single();
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyNotFoundException))]
        public void MissingProperty()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Byte", typeof(byte));
            table.Columns.Add("Integer", typeof(int));
            table.Columns.Add("FloatingPoint", typeof(double));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Id", typeof(Guid));
            table.Rows.Add((byte)23, 23, 2.3, "twenty three", new DateTime(1999, 2, 3),
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"));

            // Act
            table.ToObject<SomeData>().Single();
        }

        [TestMethod]
        public void MissingPropertySafe()
        {
            // Arrange
            var table = new DataTable();
            table.Columns.Add("Byte", typeof(byte));
            table.Columns.Add("Integer", typeof(int));
            table.Columns.Add("FloatingPoint", typeof(double));
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Id", typeof(Guid));
            table.Rows.Add((byte)23, 23, 2.3, "twenty three", new DateTime(1999, 2, 3),
                new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"));

            // Act
            table.ToObjectSafe<SomeData>().Single();
        }
    }
}
