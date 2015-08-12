namespace DataTableToObject.Exceptions
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The corresponding type in the specified class is different than the one found in the DataTable.
    /// </summary>
    public class MismatchedTypesException : Exception
    {
        internal MismatchedTypesException(PropertyInfo property, Type column) : base(
            string.Format(
                "The property \"{0}\" of the given class is of type \"{1}\" but the corresponding property in the DataTable is \"{2}\".",
                property.Name,
                property.PropertyType.Name,
                column.Name
            )
        ) { }
    }
}