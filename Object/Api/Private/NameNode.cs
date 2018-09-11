namespace ObjectLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    internal class NameNode
    {
        internal readonly string Name;

        internal readonly string FullName;

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
