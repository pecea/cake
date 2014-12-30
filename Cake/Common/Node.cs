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
            Paths = new List<string>();
        }

        public Node(string value, string[] splitPath)
            : this(value)
        {
            SplitPath = splitPath;
        }

        public static List<string> Paths { get; private set; }

        private static string[] SplitPath { get; set; }

        private string Value { get; set; }

        private List<Node> Children { get; set; }

        public void ResolveNode(GetPathsOptions option, int pathIndex = 0)
        {
            var nextPathPart = SplitPath[pathIndex + 1];

            if (String.IsNullOrEmpty(nextPathPart)) return;

            if (pathIndex + 2 == SplitPath.Length && option == GetPathsOptions.Files)
            {
                var files = Directory.GetFiles(Value, nextPathPart);
                Paths.AddRange(files);
                return;
            }

            if (nextPathPart == "**")
            {
                var directories = Directory.GetDirectories(Value);
                Children.AddRange(directories.Select(directory => new Node(directory)));
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
                Paths.AddRange(Children.Select(child => child.Value));
                return;
            }

            foreach (var child in Children) child.ResolveNode(option, pathIndex + 1);
        }
    }
}