namespace ObjectLibrary
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Couldn't find a value for a property.
    /// </summary>
    [Serializable]
    public class ValueNotFoundException : ObjectException
    {
        internal ValueNotFoundException(Type classType, string propertyName)
            : base(string.Format(CultureInfo.InvariantCulture,
            "Couldn't find a value for the property \"{0}\" of class \"{1}\".", propertyName, classType.Name)) { }
    }
}