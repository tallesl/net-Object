namespace ToObject.Tests
{
    using System;

    public class SomeData
    {
        public Guid? Id { get; set; }

        public DateTime? Date { get; set; }

        public string Text { get; set; }

        public SomeData Nested { get; set; }
    }
}
