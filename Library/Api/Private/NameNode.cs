namespace ObjectLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class NameNode
    {
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        public readonly string Name;

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        public readonly string FullName;

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        public readonly ReadOnlyCollection<NameNode> Children;

        internal NameNode(string name, string fullname, IEnumerable<NameNode> child)
        {
            Name = name;
            FullName = fullname;
            Children = new ReadOnlyCollection<NameNode>(child.ToList());
        }

        public bool EndNode
        {
            get
            {
                return !Children.Any();
            }
        }
    }
}
