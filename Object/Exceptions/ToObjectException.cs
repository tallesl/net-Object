namespace ObjectLibrary
{
    using System;

    /// <summary>
    /// Base exception.
    /// </summary>
    [Serializable]
    public abstract class ObjectException : Exception
    {
        internal ObjectException(string message) : base(message) { }
    }
}