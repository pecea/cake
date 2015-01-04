namespace Common
{
    /// <summary>
    /// Enum used in <see cref="Node.ResolveNode"/> to specify whether we are looking for directories or files.
    /// </summary>
    public enum GetPathsOptions
    {
        /// <summary>
        /// Look for directories.
        /// </summary>
        Directories,
        /// <summary>
        /// Look for files.
        /// </summary>
        Files
    }
}