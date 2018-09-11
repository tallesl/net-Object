namespace ObjectLibrary
{
    using System;

    /// <summary>
    /// Base exception.
    /// </summary>
    public abstract class ObjectException : Exception
    {
        internal ObjectException(string message) : base(message) { }
    }
}