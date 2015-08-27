namespace ToObject.Exceptions
{
    using System;

    /// <summary>
    /// Couldn't find a property in class.
    /// </summary>
    public class PropertyNotFoundException : ToObjectException
    {
        internal PropertyNotFoundException(Type classType, string propertyName)
            : base(string.Format("Couldn't find the property \"{0}\" in class \"{1}\".", propertyName, classType.Name)) { }
    }
}