namespace Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Node
    {
        private Node(string value)
        {
            Value = value;
            Children = new List<Node>();
        }

        /// <summary>
        /// Creates a new <see cref="Node"/>.
        /// </summary>
        /// <param name="value">Node's value, resolved path so far.</param>
        /// <param name="splitPath">Whole path to be resolved split by '\'.</param>
        public Node(string value, string[] splitPath)
            : this(value)
        {
            SplitPath = splitPath;
        }

        private static string[] SplitPath { get; set; }

        private string Value { get; set; }

        private List<Node> Children { get; set; }

        /// <summary>
        /// Resolves a node by finding all subdirectories or files matching a wildcard.
        /// </summary>
        /// <param name="option"><see cref="GetPathsOptions"/> specifying whether directories or files are to be found.</param>
        /// <param name="pathIndex">Index of the tree level we are currently on.</param>
        /// <returns>Enumeration of paths found that match the wildcard.</returns>
        public IEnumerable<string> ResolveNode(GetPathsOptions option, int pathIndex = 0)
        {
            Logger.Log(LogLevel.Trace, "ResolveNode method started");
            var result = new List<string>();
            var nextPathPart = SplitPath[pathIndex + 1];

            if (string.IsNullOrEmpty(nextPathPart)) return result;

            if (pathIndex + 2 == SplitPath.Length && option == GetPathsOptions.Files)
            {
                var files = Directory.GetFiles(Value, nextPathPart);
                result.AddRange(files);
                return result;
            }

            if (nextPathPart == "**")
            {
                var directories = Directory.GetDirectories(Value, "*", SearchOption.AllDirectories);
                Children.AddRange(directories.Select(directory => new Node(directory)));
                Children.Add(new Node(Value));
            }
            else if (nextPathPart.Contains("*"))
            {
                var directories = Directory.GetDirectories(Value, nextPathPart);
                Children.AddRange(directories.Select(directory => new Node(directory)));
            }
            else
            {
                var directory = $"{Value}\\{nextPathPart}";
                if (Directory.Exists(directory)) Children.Add(new Node(directory));
            }

            if (pathIndex + 2 == SplitPath.Length)
            {
                result.AddRange(Children.Select(child => child.Value));
                return result;
            }

            foreach (var child in Children) result.AddRange(child.ResolveNode(option, pathIndex + 1));

            return result;
        }
    }
}