namespace DataTableToObject.Exceptions
{
    using System;

    /// <summary>
    /// Base exception.
    /// </summary>
    public abstract class DataTableToObjectException : Exception
    {
        internal DataTableToObjectException(string message) : base(message) { }
    }
}