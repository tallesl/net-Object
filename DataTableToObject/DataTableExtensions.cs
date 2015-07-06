namespace DataTableToObject
{
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
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the DataRow is not found on the type
        /// </exception>
        public static T ToObject<T>(this DataRow row) where T : new()
        {
            return ToObject<T>(row, false);
        }

        /// <summary>
        /// Parses the DataRow to the given type.
        /// Doesn't throw if a property in the DataRow is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="row">The row to parse</param>
        /// <returns>The parsed object</returns>
        public static T ToObjectSafe<T>(this DataRow row) where T : new()
        {
            return ToObject<T>(row, true);
        }

        /// <summary>
        /// Parses the DataTable to an IEnumerable of the given type.
        /// Throws PropertyNotFoundException if a property in the DataTable is not found on the type.
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="table">The table to parse</param>
        /// <returns>The parsed objects</returns>
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the DataTable is not found on the type
        /// </exception>
        public static IEnumerable<T> ToObject<T>(this DataTable table) where T : new()
        {
            foreach (DataRow row in table.Rows) yield return ToObject<T>(row, false);
        }

        /// <summary>
        /// Parses the DataTable to an IEnumerable of the given type.
        /// Doesn't throw if a property in the DataTable is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="table">The table to parse</param>
        /// <returns>The parsed objects</returns>
        public static IEnumerable<T> ToObjectSafe<T>(this DataTable table) where T : new()
        {
            foreach (DataRow row in table.Rows) yield return ToObject<T>(row, true);
        }

        private static T ToObject<T>(DataRow row, bool safe) where T : new()
        {
            var o = new T();
            foreach (DataColumn column in row.Table.Columns)
            {
                var className = typeof(T).Name;
                var propertyName = column.ColumnName;
                var info = o.GetType().GetProperty(propertyName);

                if (info == null && !safe) throw new PropertyNotFoundException(className, propertyName);
                else if (info != null && row[column] != DBNull.Value) info.SetValue(o, row[column], null);
            }
            return o;
        }
    }
}