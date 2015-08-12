namespace DataTableToObject
{
    using DataTableToObject.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Extension methods for parsing DataRow/DataTable to a custom class.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Parses the DataRow to the given type.
        /// Throws PropertyNotFoundException if a property in the DataRow is not found on the type.
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="row">The row to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given row is null</exception>
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the DataRow is not found on the type
        /// </exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObject<T>(this DataRow row) where T : new()
        {
            if (row == null) throw new ArgumentNullException("row");
            return ToObject<T>(row, false);
        }

        /// <summary>
        /// Parses the DataRow to the given type.
        /// Doesn't throw if a property in the DataRow is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="row">The row to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given row is null</exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObjectSafe<T>(this DataRow row) where T : new()
        {
            if (row == null) throw new ArgumentNullException("row");
            return ToObject<T>(row, true);
        }

        /// <summary>
        /// Parses the DataTable to an IEnumerable of the given type.
        /// Throws PropertyNotFoundException if a property in the DataTable is not found on the type.
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="table">The table to parse</param>
        /// <returns>The parsed objects</returns>
        /// <exception cref="ArgumentNullException">If the given table is null</exception>
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the DataTable is not found on the type
        /// </exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static IEnumerable<T> ToObject<T>(this DataTable table) where T : new()
        {
            if (table == null) throw new ArgumentNullException("table");
            foreach (DataRow row in table.Rows) yield return ToObject<T>(row, false);
        }

        /// <summary>
        /// Parses the DataTable to an IEnumerable of the given type.
        /// Doesn't throw if a property in the DataTable is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="table">The table to parse</param>
        /// <returns>The parsed objects</returns>
        /// <exception cref="ArgumentNullException">If the given table is null</exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static IEnumerable<T> ToObjectSafe<T>(this DataTable table) where T : new()
        {
            if (table == null) throw new ArgumentNullException("table");
            foreach (DataRow row in table.Rows) yield return ToObject<T>(row, true);
        }

        private static T ToObject<T>(DataRow row, bool safe) where T : new()
        {
            var o = new T();
            foreach (DataColumn column in row.Table.Columns)
            {
                // Iteration values
                var currentType = o.GetType();
                object previousValue = null;
                object currentValue = o;

                // Iterating "foo" then "bar" then "qux" on "foo.bar.qux"
                foreach (var propertyName in column.ColumnName.Split(new[] { '.' }))
                {
                    // Getting the current property
                    var currentProperty = currentType.GetProperty(propertyName);

                    // Checking if the property exists
                    if (currentProperty == null)
                    {
                        if (safe) break;
                        else throw new PropertyNotFoundException(o.GetType(), column.ColumnName);
                    }

                    // Getting the current type
                    currentType = Nullable.GetUnderlyingType(currentProperty.PropertyType) ?? currentProperty.PropertyType;

                    // Finding out if it's a custom class that we can instantiate it
                    var instantiatedByUs = currentType.GetConstructor(Type.EmptyTypes) != null;

                    // Storing previous value
                    previousValue = currentValue;

                    // Getting the current value
                    currentValue = currentProperty.GetValue(previousValue, null);
                    if (instantiatedByUs && currentValue == null) currentValue = Activator.CreateInstance(currentType);

                    // Checking if the types match
                    if (!instantiatedByUs && currentType != column.DataType)
                        throw new MismatchedTypesException(currentProperty, column.DataType);

                    // Setting the current value on the current property
                    var valueToSet = instantiatedByUs ? currentValue :
                        (row[column] == DBNull.Value ? null : row[column]);
                    currentProperty.SetValue(previousValue, valueToSet, null);
                }
            }
            return o;
        }
    }
}