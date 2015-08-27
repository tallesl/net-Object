namespace ToObject
{
    using NameTrees;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ToObject.Exceptions;

    public static partial class ToObjectExtensions
    {
        private static T ToObject<T>(IDictionary<string, object> dict, bool safe)
        {
            if (dict == null)
                throw new ArgumentNullException("dict");

            var tree = new NameTree(dict.Keys);

            return (T)ToObject(typeof(T), dict, safe, tree.Root.Children);
        }

        private static object ToObject(Type t, IDictionary<string, object> dict, bool safe, IEnumerable<NameNode> children)
        {
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

                        // We just use the value in the dictionary
                        dict[child.Fullname] :

                        // Else
                        (
                            // If any of its children has value
                            AnyChildWithValue(dict, child.Children) ?

                            // We call this very method again on it (recursion)
                            ToObject(property.PropertyType, dict, safe, child.Children) :

                            // Else we just use null
                            null
                        );

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

        // The property type, if it's nullable we get its underlying type
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