namespace DataTableToObject
{
    using System;

    /// <summary>
    /// Exception throw when a column of a DataRow/DataTable doesn't match any in the given class.
    /// </summary>
    public class PropertyNotFoundException : Exception
    {
        internal PropertyNotFoundException(string className, string propertyName)
            : base(string.Format("Couldn't find the property \"{0}\" in class \"{1}\".", propertyName, className)) { }
    }
}