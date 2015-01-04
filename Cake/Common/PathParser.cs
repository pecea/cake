namespace Common
{
    using System;
    using System.Collections.Generic;

    public static class PathParser
    {

        public static IEnumerable<string> GetFolderPaths(this String path)
        {
            return GetPaths(path, GetPathsOptions.Folders);
        }

        public static IEnumerable<string> GetFilePaths(this String path)
        {
            return GetPaths(path, GetPathsOptions.Files);
        }

        private static IEnumerable<string> GetPaths(string path, GetPathsOptions option)
        {
            if (!path.Contains("*")) return new[] { path };

            var splitPath = path.Split('\\', '/');
            var pathsRoot = new Node(splitPath[0], splitPath);
            return pathsRoot.ResolveNode(option);
        }
    }
}