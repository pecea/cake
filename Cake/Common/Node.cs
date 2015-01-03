namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Node
    {
        private Node(string value)
        {
            Value = value;
            Children = new List<Node>();
        }

        public Node(string value, string[] splitPath)
            : this(value)
        {
            SplitPath = splitPath;
        }

        private static string[] SplitPath { get; set; }

        private string Value { get; set; }

        private List<Node> Children { get; set; }

        public IEnumerable<string> ResolveNode(GetPathsOptions option, int pathIndex = 0)
        {
            var result = new List<string>();
            var nextPathPart = SplitPath[pathIndex + 1];

            if (String.IsNullOrEmpty(nextPathPart)) return result;

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
                Children.Add(new Node(String.Format("{0}\\{1}", Value, nextPathPart)));
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