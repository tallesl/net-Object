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
                var columnType = column.DataType;
                var columnName = column.ColumnName;
                var columnValue = row[column];

                var classType = o.GetType();
                var className = classType.Name;

                var property = classType.GetProperty(columnName);

                // class property name != DataTable column name
                if (property == null)
                {
                    // if it's not safe we throw
                    if (!safe) throw new PropertyNotFoundException(className, columnName);
                }
                else
                {
                    var propertyType = property.PropertyType;

                    // class property type != DataTable column type
                    if (columnType != propertyType) throw new MismatchedTypesException(propertyType, columnType);

                    // Both names and types have matched, good to go
                    if (row[column] != DBNull.Value) property.SetValue(o, columnValue, null);
                }
            }
            return o;
        }
    }
}