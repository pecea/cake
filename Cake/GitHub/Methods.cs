using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using System.IO;

namespace GitHub
{
    /// <summary>
    /// 
    /// </summary>
    public static class Methods
    {
        private static string _oauthToken;
        private static string _releaseNotesFile;
        private static string _repository;
        private static string _tagName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static ReleaseAssetUpload BuildAssetUpload(UploadFile item)
        {
            return new ReleaseAssetUpload { ContentType = item.ContentType ?? "application/octet-stream", FileName = Path.GetFileName(item.Path), RawData = File.OpenRead(item.Path) };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static ReleaseUpdate BuildReleaseData()
        {
            var update = new ReleaseUpdate(_tagName);
            if (_releaseNotesFile != null)
            {
                update.Body = File.ReadAllText(_releaseNotesFile);
            }
            return update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="oauthToken"></param>
        /// <param name="tagName"></param>
        /// <param name="files"></param>
        /// <param name="releaseNotesFile"></param>
        /// <returns></returns>
        public static bool Release(string repository, string oauthToken, string tagName, UploadFile[] files, string releaseNotesFile = null)
        {
            _repository = repository;
            _oauthToken = oauthToken;
            _tagName = tagName;
            _releaseNotesFile = releaseNotesFile;
            var client = new GitHubClient(new ProductHeaderValue("GitTfsTasks"), CredentialStore).Release;
            var result = client.CreateRelease(Owner, RepositoryName, BuildReleaseData()).Result;
            IdRelease = result.Id;
            if ((files != null) && (files.Length != 0))
            {
                UploadedAssets = UploadAll(client, result, files);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="release"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static string TaskItemFor(Octokit.Release release, Task<ReleaseAsset> asset)//what is release parameter for?
        {
            return ("https://github.com/" + _repository + "/releases/download/" + _tagName + "/" + asset.Result.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="release"></param>
        /// <param name="sourceItem"></param>
        /// <returns></returns>
        private static string Upload(IReleasesClient client, Release release, UploadFile sourceItem)
        {
            var asset = client.UploadAsset(release, BuildAssetUpload(sourceItem));
            return TaskItemFor(release, asset);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="release"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private static string[] UploadAll(IReleasesClient client, Release release, IEnumerable<UploadFile> items)
        {
            return (from item in items select Upload(client, release, item)).ToArray<string>();
        }

        private static ICredentialStore CredentialStore
        {
            get
            {
                return new InPlaceCredentialStore(_oauthToken);
            }
        }
        
        public static int IdRelease { get; private set; }
        
        private static string Owner
        {
            get
            {
                return _repository.Split(new char[] { '/' })[0];
            }
        }
        
        private static string RepositoryName
        {
            get
            {
                return _repository.Split(new char[] { '/' })[1];
            }
        }
        
        public static string[] UploadedAssets { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        private class InPlaceCredentialStore : ICredentialStore
        {
            private static string _token;
            
            public InPlaceCredentialStore(string token)
            {
                _token = token;
            }
            
            public async Task<Credentials> GetCredentials()
            {
                return new Credentials(_token);
            }
            
            //[CompilerGenerated]
            //private struct <GetCredentials>d__4 : IAsyncStateMachine
            //{
            //    public static int <>1__state;
            //    public static InPlaceCredentialStore <>4__this;
            //    public static AsyncTaskMethodBuilder<Credentials> <>t__builder;
                
            //    private static void MoveNext()
            //    {
            //        Credentials credentials;
            //        try
            //        {
            //            if (<>1__state != -3)
            //            {
            //                credentials = new Credentials(<>4___token);
            //            }
            //        }
            //        catch (Exception exception)
            //        {
            //            <>1__state = -2;
            //            <>t__builder.SetException(exception);
            //            return;
            //        }
            //        <>1__state = -2;
            //        <>t__builder.SetResult(credentials);
            //    }
                
            //    [DebuggerHidden]
            //    private static void SetStateMachine(IAsyncStateMachine param0)
            //    {
            //        <>t__builder.SetStateMachine(param0);
            //    }
            //}
        }
    }
}
