namespace ObjectLibrary
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The corresponding type in the specified class is different than the one found.
    /// </summary>
    public class MismatchedTypesException : ObjectException
    {
        internal MismatchedTypesException(PropertyInfo classProperty, Type foundProperty) : base(
            string.Format(
                "The property \"{0}\" of the given class is of type \"{1}\" but the corresponding property found is \"{2}\".",
                classProperty.Name,
                classProperty.PropertyType.Name,
                foundProperty.Name
            )
        ) { }
    }
}