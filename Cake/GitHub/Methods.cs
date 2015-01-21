using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using System.IO;
using Common;

namespace GitHub
{
    /// <summary>
    /// Encloses methods used with integration with github.
    /// </summary>
    public static class Methods
    {
        private static string _oauthToken;
        private static string _releaseNotesFile;
        private static string _repository;
        private static string _tagName;


        private static ReleaseAssetUpload BuildAssetUpload(UploadFile item)
        {
            return new ReleaseAssetUpload { ContentType = item.ContentType ?? "application/octet-stream", FileName = Path.GetFileName(item.Path), RawData = File.OpenRead(item.Path) };
        }


        private static NewRelease BuildReleaseData()
        {
            var update = new NewRelease(_tagName);
            if (_releaseNotesFile != null)
            {
                update.Body = File.ReadAllText(_releaseNotesFile);
            }
            return update;
        }

        /// <summary>
        /// Method for making a release to github.
        /// </summary>
        /// <param name="repository">URL to repository</param>
        /// <param name="oauthToken">Github oauthToken</param>
        /// <param name="tagName">Tag</param>
        /// <param name="files">Files included into release</param>
        /// <param name="releaseNotesFile">Path to file with notes for the release</param>
        /// <returns></returns>
        public static bool Release(string repository, string oauthToken, string tagName, UploadFile[] files, string releaseNotesFile = null)
        {
            _repository = repository;
            _oauthToken = oauthToken;
            _tagName = tagName;
            _releaseNotesFile = releaseNotesFile;

            try
            {
                var client = new GitHubClient(new ProductHeaderValue("GitTfsTasks"), CredentialStore).Release;
                var result = client.Create(Owner, RepositoryName, BuildReleaseData()).Result;
                IdRelease = result.Id;
                if ((files != null) && (files.Length != 0))
                {
                    files = (UploadFile[])files.Where((f) => (f.ContentType != null) && File.Exists(f.Path));
                    //var upAssets = new List<string>();
                    //foreach (var file in files)
                    //{
                    //    try
                    //    {
                    //        upAssets.Add(Upload(client, result, file));
                    //        Logger.Log(LogLevel.Debug, "File "+Path.GetFileName(file.Path)+"uploaded correctly");
                    //    }
                    //    catch (Exception)
                    //    {
                    //        Logger.Log(LogLevel.Warn, "File "+Path.GetFileName(file.Path)+" not uploaded!");
                    //    }


                    //}
                    UploadedAssets = UploadAll(client, result, files);
                    Logger.Log(LogLevel.Info, "Successful release!");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string TaskItemFor(Task<ReleaseAsset> asset)
        {
            return ("https://github.com/" + _repository + "/releases/download/" + _tagName + "/" + asset.Result.Name);
        }


        private static string Upload(IReleasesClient client, Release release, UploadFile sourceItem)
        {
            var asset = client.UploadAsset(release, BuildAssetUpload(sourceItem));
            return TaskItemFor(asset);
        }


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
        
        internal static int IdRelease { get; private set; }
        
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
        
        internal static string[] UploadedAssets { get; private set; }


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
        }
    }
}
