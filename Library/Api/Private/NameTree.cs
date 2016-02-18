namespace ObjectLibrary
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class NameTree
    {
        private readonly char _separator = '.';

        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "It's immutable.")]
        public readonly NameNode Root;

        public NameTree(IEnumerable<string> nameTree)
        {
            nameTree = nameTree.SelectMany(n => GetNamesInBetween(n)).Distinct();

            // Separating the names by their levels descending
            // [0] -> (level 2) "foo.bar.qux" , "foo.baz.qux"
            // [1] -> (level 1) "foo.bar" , "foo.baz"
            // [2] -> (level 0) "foo"
            var levels = (
                from name in nameTree
                group name by GetLevel(name) into g
                orderby g.Key descending
                select g
            );

            IDictionary<string, IEnumerable<NameNode>> children = new Dictionary<string, IEnumerable<NameNode>>();

            foreach (var level in levels)
            {
                // Separating the names by their 'prefix'
                // [ "foo.bar" , "foo.baz" ] -> "foo": [ "bar" , "baz" ]
                var names = (
                    from name in level.ToList()
                    group name by GetPrefix(name) into g
                    orderby g.Key
                    select g
                ).ToDictionary(g => g.Key, g => g.ToList() as IEnumerable<string>);

                children = GetChildren(children, names);
            }

            // If there's only one child it's our root, else we create a empty root as their parents
            var childrenLeft = children.Values.SelectMany(c => c);
            Root = childrenLeft.Count() == 1 ? childrenLeft.Single() :
                new NameNode(string.Empty, string.Empty, childrenLeft);
        }

        // Returns [ "foo", "foo.bar", "foo.bar.qux" ] on "foo.bar.qux"
        private IEnumerable<string> GetNamesInBetween(string name)
        {
            var parts = name.Split(_separator);
            var last = string.Empty;
            for (var i = 0; i < parts.Length; ++i)
            {
                var current = last == string.Empty ? parts[i] : last + _separator + parts[i];
                yield return current;
                last = current;
            }
        }

        // Returns the level (how many 'dots') of the name
        private int GetLevel(string name)
        {
            return name.Count(c => c == _separator);
        }

        // Returns "foo.bar" on "foo.bar.qux".
        private string GetPrefix (string name)
        {
            var last = name.LastIndexOf(_separator);
            return last > 0 ? name.Substring(0, last) : string.Empty;
        }

        // Returns "bar" on "foo.bar.qux".
        private string GetSufix (string name)
        {
            return name.Substring(name.LastIndexOf(_separator) + 1);
        }

        // Constructs the children of the given prefix, children names and grandchildren
        private IDictionary<string, IEnumerable<NameNode>> GetChildren(
            IDictionary<string, IEnumerable<NameNode>> previousChildren,
            IDictionary<string, IEnumerable<string>> names)
        {
            var children = new Dictionary<string, IEnumerable<NameNode>>();

            foreach (var parentAndChildren in names)
                children.Add(
                    // Prefix of the names (parent)
                    parentAndChildren.Key,

                    GetNodes(
                        // Fullnames
                        parentAndChildren.Value,

                        // Children's children (grandchildren)
                        previousChildren
                    )
                );

            return children;
        }

        // Constructs the nodes of the given names and children
        private IEnumerable<NameNode> GetNodes(
            IEnumerable<string> fullNames,
            IDictionary<string, IEnumerable<NameNode>> children)
        {
            foreach (var fullname in fullNames)
                yield return new NameNode(
                    GetSufix(fullname),
                    fullname,
                    children.ContainsKey(fullname) ? children[fullname] : Enumerable.Empty<NameNode>()
                );
        }
    }
}
