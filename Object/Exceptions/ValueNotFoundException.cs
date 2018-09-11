namespace ObjectLibrary
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Couldn't find a value for a property.
    /// </summary>
    public class ValueNotFoundException : ObjectException
    {
        internal ValueNotFoundException(Type classType, string propertyName)
            : base($"Couldn't find a value for the property \"{propertyName}\" of class \"{classType.Name}\".") { }
    }
}