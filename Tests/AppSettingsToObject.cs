namespace ToObject.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using ToObject.Exceptions;

    [TestClass]
    public class AppSettingsToObject
    {
        [TestMethod]
        public void LengthyTest()
        {
            var expected =
                new SomeData
                {
                    Id = new Guid("366f4bd3-6717-4b14-9c79-70515296df7e"),
                    Date = new DateTime(1999, 1, 1),
                    Text = "level 1",
                    Nested =
                        new SomeData
                        {
                            Id = new Guid("e591be31-289f-4a99-ba67-288ea24b7d7e"),
                            Date = new DateTime(1999, 2, 2),
                            Text = "level 2",
                            Nested =
                                new SomeData
                                {
                                    Id = null,
                                    Date = null,
                                    Text = null,
                                    Nested =
                                        new SomeData
                                        {
                                            Id = new Guid("3bfdd62f-8b31-4aa2-931d-46535f291b0e"),
                                            Date = new DateTime(1999, 4, 4),
                                            Text = "level 4",
                                            Nested = null,
                                        },
                                },
                        },
                };

            var actual = ConfigurationManager.AppSettings.ToObject<SomeData>();

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
    }
}
