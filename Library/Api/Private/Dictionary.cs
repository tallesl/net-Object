﻿namespace ToObject
{
    using NameTrees;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using ToObject.Exceptions;

    public static partial class ToObjectExtensions
    {
        private static T ToObject<T>(IDictionary<string, object> dict, bool safe, bool parse)
        {
            if (dict == null)
                throw new ArgumentNullException("dict");

            var tree = new NameTree(dict.Keys);

            return (T)ToObject(typeof(T), dict, tree.Root.Children, safe, parse);
        }

        private static object ToObject(
            Type t, IDictionary<string, object> dict, IEnumerable<NameNode> children, bool safe, bool parse)
        {
            // If there's no child with value we don't instantiate
            if (!AnyChildWithValue(dict, children)) return null;

            // Getting an instance of the object being created
            var beingCreated = Convert.ChangeType(Activator.CreateInstance(t, false), t);

            // Iterating its children
            foreach (var child in children ?? Enumerable.Empty<NameNode>())
            {
                // Getting the PropertyInfo of the type that matches the child's name (reflection)
                var property = beingCreated.GetType().GetProperty(child.Name);

                // If there's no such property in the class
                if (property == null)
                {
                    // If it's not safe
                    if (!safe)

                        // We throw
                        throw new PropertyNotFoundException(beingCreated.GetType(), child.Name);
                }
                else
                {
                    // The value to set on the property
                    var value =

                        // If it's an end node (child without children)
                        child.EndNode ?

                        // We use the value in the dictionary and convert it if needed
                        (parse ? Parse((string)dict[child.Fullname], property.PropertyType) : dict[child.Fullname]) :

                        // Else we call this very method again on it (recursion)
                        ToObject(property.PropertyType, dict, child.Children, safe, parse);

                    // If types on the dictionary and the class doesn't match
                    if (value != null && value.GetType() != GetUnderlyingType(property))

                        // We throw (despite being safe or not)
                        throw new MismatchedTypesException(property, value.GetType());
                    
                    // Setting the property value (reflection)
                    property.SetValue(beingCreated, value, null);
                }
            }

            // Here, take it :)
            return beingCreated;
        }

        // Returns if the given type accepts null
        private static bool AcceptsNull(Type t)
        {
            return !t.IsValueType || Nullable.GetUnderlyingType(t) != null;
        }

        // Attempts to parse the given string to the give type
        private static object Parse(string value, Type t)
        {
            if (t == typeof(string)) return value;
            else if (value == null && !AcceptsNull(t)) throw new CouldntParseException(value, t);
            else
            {
                var converter = TypeDescriptor.GetConverter(t);
                try
                {
                    return converter.ConvertFromString(value);
                }
                catch
                {
                    throw new CouldntParseException(value, t);
                }
            }
        }

        // Returns the property type, if it's nullable we get its underlying type
        private static Type GetUnderlyingType(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        }

        // Navigates the children finding out if there's any that's not null
        private static bool AnyChildWithValue(IDictionary<string, object> dict, IEnumerable<NameNode> children)
        {
            return children.Where(c => c.EndNode).Any(c => dict[c.Fullname] != null) ||
                children.Where(c => !c.EndNode).Any(c => AnyChildWithValue(dict, c.Children));
        }
    }
}