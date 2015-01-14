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

        public string ContentType { get; set; }

        public string Path { get; set; }
    }
}
