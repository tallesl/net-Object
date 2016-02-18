namespace ObjectLibrary
{
    using System;

    /// <summary>
    /// Couldn't find a value for a property.
    /// </summary>
    public class ValueNotFoundException : ObjectException
    {
        internal ValueNotFoundException(Type classType, string propertyName)
            : base(string.Format("Couldn't find a value for the property \"{0}\" of class \"{1}\".", propertyName, classType.Name)) { }
    }
}