namespace ToObject.Tests.Data
{
    using System;

    public class RecursiveData
    {
        public Guid? Id { get; set; }

        public DateTime? Date { get; set; }

        public string Text { get; set; }

        public RecursiveData Nested { get; set; }
    }
}
