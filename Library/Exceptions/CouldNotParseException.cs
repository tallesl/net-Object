namespace ObjectLibrary
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Couldn't parse the found string to the found type.
    /// </summary>
    public class CouldNotParseException : ObjectException
    {
        internal CouldNotParseException(string value, Type type) : base(
            string.Format(
                CultureInfo.InvariantCulture,
                "Couldn't parse \"{0}\" to type \"{1}\".",
                value,
                type.Name
            )
        ) { }
    }
}