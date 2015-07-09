namespace DataTableToObject.Exceptions
{
    using System;

    /// <summary>
    /// The corresponding type in the specified class is different than the one found in the DataTable.
    /// </summary>
    public class MismatchedTypesException : Exception
    {
        internal MismatchedTypesException(Type propertyType, Type columnType) : base(string.Format(
            "The property of the class is of type \"{0}\" but the corresponding property in the DataTable is \"{1}\".",
            propertyType.Name, columnType.Name)) { }
    }
}