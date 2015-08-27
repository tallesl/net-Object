namespace ToObject.Exceptions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Couldn't parse the found string to the found type.
    /// </summary>
    public class CouldntParseException : ToObjectException
    {
        internal CouldntParseException(string value, Type type)
            : base(string.Format("Couldn't parse \"{0}\" to type \"{1}\".", value, type.Name)) { }
    }
}