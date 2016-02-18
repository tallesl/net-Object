namespace ObjectLibrary
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// The corresponding type in the specified class is different than the one found.
    /// </summary>
    [Serializable]
    public class MismatchedTypesException : ObjectException
    {
        internal MismatchedTypesException(PropertyInfo classProperty, Type foundProperty) : base(
            string.Format(
                CultureInfo.InvariantCulture,
                "The property \"{0}\" of the given class is of type \"{1}\" but the corresponding property found is \"{2}\".",
                classProperty.Name,
                classProperty.PropertyType.Name,
                foundProperty.Name
            )
        ) { }
    }
}