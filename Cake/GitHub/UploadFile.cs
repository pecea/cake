namespace GitHub
{
    /// <summary>
    /// Class representing a file for github release
    /// </summary>
    public class UploadFile
    {
        public UploadFile(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Type of content, eg. application/zip
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Path to file
        /// </summary>
        public string Path { get; set; }
    }
}
