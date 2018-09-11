namespace ObjectLibrary
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// The corresponding type in the specified class is different than the one found.
    /// </summary>
    public class MismatchedTypesException : ObjectException
    {
        internal MismatchedTypesException(PropertyInfo classProperty, Type foundProperty) : base(
            $"The property \"{classProperty.Name}\" of the given class is of type " +
            $"\"{classProperty.PropertyType.Name}\" but the corresponding property found is \"{foundProperty.Name}\".")
            { }
    }
}