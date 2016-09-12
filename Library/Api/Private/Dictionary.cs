namespace ObjectLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public static partial class ObjectExtensions
    {
        private static object ToObject(Type t, IDictionary<string, string> dict, bool safe, bool parse)
        {
            return ToObject(t, dict.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value), safe, parse);
        }

        private static object ToObject(Type t, IDictionary<string, object> dict, bool safe, bool parse)
        {
            var tree = new NameTree(dict.Keys);
            var children = dict.Keys.Count == 1 ? (IEnumerable<NameNode>)new[] { tree.Root } : tree.Root.Children;

            return ToObject(t, dict, children, safe, parse);
        }

        private static object ToObject(
            Type t, IDictionary<string, object> dict, IEnumerable<NameNode> children, bool safe, bool parse)
        {
            // If there's no child with value we don't instantiate
            if (!AnyChildWithValue(dict, children))
                return null;

            // Getting an instance of the object being created
            var beingCreated = Convert.ChangeType(Activator.CreateInstance(t, false), t, CultureInfo.InvariantCulture);

            // Names of the properties in the class that has been assigned
            var assigned = new HashSet<string>();

            // Iterating its children
            foreach (var child in children ?? Enumerable.Empty<NameNode>())
            {
                // Getting the PropertyInfo of the type that matches the child's name (reflection)
                var property = t.GetProperty(child.Name);

                // If there's no such property in the class
                if (property == null)
                {
                    // If it's not safe
                    if (!safe)

                        // We throw
                        throw new PropertyNotFoundException(t, child.Name);
                }
                else
                {
                    // The value to set on the property
                    var value =

                        // If it's an end node (child without children)
                        child.EndNode ?

                        // We use the value in the dictionary and convert it if needed
                        (parse ? Parse((string)dict[child.FullName], property.PropertyType) : dict[child.FullName]) :

                        // Else we call this very method again on it (recursion)
                        ToObject(property.PropertyType, dict, child.Children, safe, parse);

                    // If it's nullable we get its underlying type
                    var underlying =
                        Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    // If the type is an enum and the value is an enum convertible type and isn't null
                    if (value != null && underlying.IsEnum && _enumTypes.Contains(value.GetType()))

                        // We set the value to the corresponding one in the enum
                        value = Enum.ToObject(underlying, value);

                    // If types on the dictionary and the class doesn't match
                    if (value != null && value.GetType() != underlying)

                        // We throw (despite being safe or not)
                        throw new MismatchedTypesException(property, value.GetType());

                    // Setting the property value (reflection)
                    property.SetValue(beingCreated, value, null);

                    // Adding to our assigned collection (we'll check those later)
                    assigned.Add(property.Name);
                }
            }

            // If it's not safe
            if (!safe)
            {
                // Getting the properties that hasn't been assigned
                var notAssigned = t.GetProperties().Select(p => p.Name).Except(assigned);

                // If there's any
                if (notAssigned.Any())

                    // We throw
                    throw new ValueNotFoundException(t, notAssigned.First());
            }

            // Here, take it :)
            return beingCreated;
        }

        // Possible enum underlying types
        // https://msdn.microsoft.com/library/sbbt4032.aspx
        private static Type[] _enumTypes =
            new[]
            {
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
            };

        // Returns if the given type accepts null
        private static bool AcceptsNull(Type t)
        {
            return !t.IsValueType || Nullable.GetUnderlyingType(t) != null;
        }

        // Attempts to parse the given string to the given type
        private static object Parse(string value, Type t)
        {
            if (t == typeof(string))
                return value;
            else if (value == null && !AcceptsNull(t))
                throw new CouldNotParseException(value, t);
            else
            {
                if (t.IsArray)
                {
                    if (value == null)
                        return null;

                    var underlyingType = t.GetElementType();
                    var tokens = value.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                    var array = Array.CreateInstance(underlyingType, tokens.Length);

                    for (var i = 0; i < tokens.Length; ++i)
                        array.SetValue(TypeDescriptor.GetConverter(underlyingType).ConvertFromString(tokens[i].Trim()), i);

                    return array;
                }
                else
                {
                    try
                    {
                        return TypeDescriptor.GetConverter(t).ConvertFromString(value);
                    }
                    catch
                    {
                        throw new CouldNotParseException(value, t);
                    }
                }
            }
        }

        // Navigates the children finding out if there's any that's not null
        private static bool AnyChildWithValue(IDictionary<string, object> dict, IEnumerable<NameNode> children)
        {
            return children.Where(c => c.EndNode).Any(c => dict[c.FullName] != null) ||
                children.Where(c => !c.EndNode).Any(c => AnyChildWithValue(dict, c.Children));
        }
    }
}