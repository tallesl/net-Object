namespace ObjectLibrary
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Couldn't find a property in class.
    /// </summary>
    public class PropertyNotFoundException : ObjectException
    {
        internal PropertyNotFoundException(Type classType, string propertyName) :
            base($"Couldn't find the property \"{propertyName}\" in class \"{classType.Name}\".") { }
    }
}