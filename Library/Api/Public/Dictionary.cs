namespace ToObject
{
    using System;
    using System.Collections.Generic;
    using ToObject.Exceptions;

    public static partial class ToObjectExtensions
    {
        /// <summary>
        /// Parses the dictionary to the given type.
        /// Throws PropertyNotFoundException if a property in the dictionary is not found on the type.
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="dict">The dictionary to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given dictionary is null</exception>
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the dictionary is not found on the type
        /// </exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObject<T>(this IDictionary<string, object> dict) where T : new()
        {
            return ToObject<T>(dict, false);
        }

        /// <summary>
        /// Parses the dictionary to the given type.
        /// Doesn't throw if a key in the dictionary is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="dict">The dictionary to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given dictionary is null</exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObjectSafe<T>(this IDictionary<string, object> dict) where T : new()
        {
            return ToObject<T>(dict, true);
        }
    }
}