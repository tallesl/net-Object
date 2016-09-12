namespace ObjectLibrary
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;

    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Parses the NameValueCollection to the given type.
        /// Throws PropertyNotFoundException if a key in the NameValueCollection is not found on the type.
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="namesValues">The NameValueCollection to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given NameValueCollection is null</exception>
        /// <exception cref="PropertyNotFoundException">
        /// If a property in the NameValueCollection is not found on the type
        /// </exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObject<T>(this NameValueCollection namesValues) where T : new()
        {
            if (namesValues == null)
                throw new ArgumentNullException("namesValues");

            var dict = namesValues.Cast<string>().ToDictionary(key => key, key => (object)namesValues[key]);
            return (T)ToObject(typeof(T), dict, false, true);
        }

        /// <summary>
        /// Parses the NameValueCollection to the given type.
        /// Doesn't throw if a key in the NameValueCollection is not found on the type (ignores it).
        /// </summary>
        /// <typeparam name="T">The type to parse to</typeparam>
        /// <param name="namesValues">The NameValueCollection to parse</param>
        /// <returns>The parsed object</returns>
        /// <exception cref="ArgumentNullException">If the given NameValueCollection is null</exception>
        /// <exception cref="MismatchedTypesException">If the found types doesn't match</exception>
        public static T ToObjectSafe<T>(this NameValueCollection namesValues) where T : new()
        {
            if (namesValues == null)
                throw new ArgumentNullException("namesValues");

            var dict = namesValues.Cast<string>().ToDictionary(key => key, key => (object)namesValues[key]);
            return (T)ToObject(typeof(T), dict, true, true);
        }
    }
}