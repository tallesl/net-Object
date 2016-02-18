namespace ObjectLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Extension methods for parsing a Dictionary/DataRow/DataTable to a custom class.
    /// </summary>
    public static partial class ObjectExtensions
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
            return ToObject<T>(table, false);
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
            return ToObject<T>(table, true);
        }
    }
}