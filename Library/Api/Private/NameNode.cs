namespace ObjectLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NameNode
    {
        public readonly string Name;

        public readonly string Fullname;

        public readonly ReadOnlyCollection<NameNode> Children;

        internal NameNode(string name, string fullname, IEnumerable<NameNode> child)
        {
            Name = name;
            Fullname = fullname;
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
