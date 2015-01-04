namespace GitHub
{
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
