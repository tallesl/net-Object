namespace ObjectLibrary
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Couldn't find a property in class.
    /// </summary>
    public class PropertyNotFoundException : ObjectException
    {
        internal PropertyNotFoundException(Type classType, string propertyName) : base(
            string.Format(
                CultureInfo.InvariantCulture,
                "Couldn't find the property \"{0}\" in class \"{1}\".",
                propertyName,
                classType.Name
            )
        ) { }
    }
}