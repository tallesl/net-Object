namespace DataTableToObject
{
    using DataTableToObject.Exceptions;
    using NameTrees;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static partial class DataTableExtensions
    {
        private static T ToObject<T>(DataRow row, bool safe)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            var table = new DataTable();
            table.ImportRow(row);
            return ToObject<T>(table, safe).First();
        }

        private static IEnumerable<T> ToObject<T>(DataTable table, bool safe)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var names = table.Columns.OfType<DataColumn>().Select(c => c.ColumnName);
            var tree = new NameTree(names);

            foreach (DataRow row in table.Rows)
                yield return (T)ToObject(typeof(T), row, safe, tree.Root.Children);
        }

        private static object ToObject(Type t, DataRow row, bool safe, IEnumerable<NameNode> children)
        {
            // Getting an instance of the object being created
            var beingCreated = Convert.ChangeType(Activator.CreateInstance(t, false), t);

            // Its type
            var beingCreatedType = beingCreated.GetType();

            // Iterating its children
            foreach (var child in children ?? Enumerable.Empty<NameNode>())
            {
                // Getting the PropertyInfo of the type that matches the child's name (reflection)
                var property = beingCreatedType.GetProperty(child.Name);

                // If there's not such property in the class
                if (property == null)
                {
                    // If it's not safe we throw (else we just ignore)
                    if (!safe)
                        throw new PropertyNotFoundException(beingCreated.GetType(), child.Name);
                }
                else
                {
                    // The property type, if it's nullable we get its underlying type (DataTable doesn't support nullable, hence the DBNull.Value thing)
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    // Getting the column in the DataTable
                    var column = row.Table.Columns[child.Fullname];

                    // If it's an end node (child without children) it has a value if the cell in the row isn't null (DBNull.Value)
                    // If it's not end node it has a value if any of its children has value
                    var hasValue = child.EndNode ? row[column] != DBNull.Value : AnyChildWithValue(row, child.Children);

                    // If it has a value and it's and end node we simply pick it from the row
                    // Else we call this very method (recursion)
                    // If there's no value null it is
                    var columnValue = hasValue ? (child.EndNode ? row[column] : ToObject(propertyType, row, safe, child.Children)) : null;

                    // If the column type and the property type aren't the same we throw (despite being safe or not)
                    if (column != null && column.DataType != propertyType)
                        throw new MismatchedTypesException(property, column.DataType);

                    // Setting the property value (reflection)
                    property.SetValue(beingCreated, columnValue, null);
                }
            }

            // Here, take it :)
            return beingCreated;
        }

        private static bool AnyChildWithValue(DataRow row, IEnumerable<NameNode> children)
        {
            foreach (var child in children)
            {
                if (child.EndNode)
                {
                    if (row[child.Fullname] != DBNull.Value)
                        return true;
                }
                else
                {
                    if (AnyChildWithValue(row, child.Children))
                        return true;
                }
            }
            return false;
        }
    }
}