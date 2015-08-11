namespace DataTableToObject.Exceptions
{
    using System;

    /// <summary>
    /// A column of a DataRow/DataTable doesn't match any in the given class.
    /// </summary>
    public class PropertyNotFoundException : Exception
    {
        internal PropertyNotFoundException(Type classType, string propertyName)
            : base(string.Format("Couldn't find the property \"{0}\" in class \"{1}\".", propertyName, classType.Name)) { }
    }
}