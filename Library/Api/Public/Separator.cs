namespace ObjectLibrary
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Extension methods for parsing a Dictionary/DataRow/DataTable to a custom class.
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Separator used when splitting a value into an array.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible",
            Justification = "That's on purpose.")]
        public static string Separator = ",";
    }
}