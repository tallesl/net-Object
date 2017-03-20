namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ObjectLibrary.Tests.Data;
    using System.Collections.Generic;

    [TestClass]
    public class SeparatorTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectExtensions.Separator = ";";
        }

        [TestCleanup]
        public void Cleanup()
        {
            ObjectExtensions.Separator = ",";
        }

        [TestMethod]
        public void StringDictionary()
        {
            // Arrange
            var expected = new RecursiveData { Array = new[] { 1, 2, 3, } };
            var dict = new Dictionary<string, string>() { { "Array", string.Join(";", expected.Array) } };

            // Act
            var actual = dict.ToObjectSafe<RecursiveData>();

            // Assert
            CollectionAssert.AreEqual(expected.Array, actual.Array);
        }
    }
}
