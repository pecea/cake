﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    /// <summary>
    /// Contains methods used to retrieve file or directory paths from a wildcarded path.
    /// </summary>
    public static class PathParser
    {

        /// <summary>
        /// Converts a wildcarded path to an enumeration of regular paths of directories.
        /// </summary>
        /// <param name="path">Wildcarded path to be converted.</param>
        /// <returns>Enumeration of directories' paths that exist and fit the wildcared path.</returns>
        public static IEnumerable<string> GetDirectoriesPaths(this string path)
        {
            return GetPaths(path, GetPathsOptions.Directories);
        }

        /// <summary>
        /// Converts a wildcarded path to an enumeration of regular paths of files.
        /// </summary>
        /// <param name="path">Wildcarded path to be converted.</param>
        /// <returns>Enumeration of files' paths that exist and fit the wildcared path.</returns>
        public static IEnumerable<string> GetFilePaths(this string path)
        {
            return GetPaths(path, GetPathsOptions.Files);
        }

        private static IEnumerable<string> GetPaths(string path, GetPathsOptions option)
        {
            Logger.LogMethodStart();
            if (!path.Contains("*"))
            {
                switch (option)
                {
                    case GetPathsOptions.Directories:
                        return Directory.Exists(path) ? new[] { path } : new string[0];
                    case GetPathsOptions.Files:
                        return File.Exists(path) ? new[] { path } : new string[0];
                    default:
                        throw new ArgumentOutOfRangeException(nameof(option));
                }
            }

            var splitPath = path.Split('\\', '/');
            var pathsRoot = new Node(splitPath[0], splitPath);
            Logger.LogMethodEnd();
            return pathsRoot.ResolveNode(option);
        }
    }
}