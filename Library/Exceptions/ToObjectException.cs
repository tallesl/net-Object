namespace ToObject.Exceptions
{
    using System;

    /// <summary>
    /// Base exception.
    /// </summary>
    public abstract class ToObjectException : Exception
    {
        internal ToObjectException(string message) : base(message) { }
    }
}