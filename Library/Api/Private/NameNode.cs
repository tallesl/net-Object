namespace ObjectLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    internal class NameNode
    {
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        internal readonly string Name;

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        internal readonly string FullName;

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "It's immutable.")]
        internal readonly ReadOnlyCollection<NameNode> Children;

        internal NameNode(string name, string fullname, IEnumerable<NameNode> child)
        {
            Name = name;
            FullName = fullname;
            Children = new ReadOnlyCollection<NameNode>(child.ToList());
        }

        internal bool EndNode
        {
            get
            {
                return !Children.Any();
            }
        }
    }
}
