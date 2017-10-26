using System;
using System.IO;
using System.Xml.Serialization;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Newtonsoft.Json;
using Git.Helpers;

namespace Git
{
    /// <summary>
    /// Encloses methods used with resolving user's git identity
    /// </summary>
    public class Identity
    {
        private string _name;
        private string _email;
        private string _username;
        private string _password;
        /// <summary>
        /// User's name
        /// </summary>
        public string Name
        {
            get => (_name = AskIfNull(_name, nameof(Name)));
            set => _name = value;
        }
        /// <summary>
        /// User's email address
        /// </summary>
        public string Email
        {
            get => (_email = AskIfNull(_email, nameof(Email)));
            set => _email = value;
        }
        /// <summary>
        /// User's username
        /// </summary>
        public string Username
        {
            get => (_username = AskIfNull(_username, nameof(Username)));
            set => _username = value;
        }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password
        {
            get => (_password = AskIfNull(_password, nameof(Password), true));
            set => _password = value;
        }

        private static string AskIfNull(string value, string name, bool masked = false)
        {
            if (!string.IsNullOrWhiteSpace(value)) return value;
            Console.WriteLine($"Please provide your git {name}:");

            return masked ? ConsoleHelper.ReadLineMasked() : Console.ReadLine();
        }
        /// <summary>
        /// Method for providing git user credentials
        /// </summary>
        public CredentialsHandler CredentialsProvider => GetCredentials;
        /// <summary>
        /// Method for providing user <see cref="LibGit2Sharp.Signature"/>
        /// </summary>
        /// <returns><see cref="LibGit2Sharp.Signature"/></returns>
        public Signature GetSignature() => new Signature(Name, Email, DateTime.Now);

        private Credentials GetCredentials(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            return new UsernamePasswordCredentials
            {
                Username = Username,
                Password = Password
            };
        }

        #region Static initializers
        /// <summary>
        /// Gets <see cref="Identity"/> from .json file
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns><see cref="Identity"/></returns>
        public static Identity FromJsonFile(string path)
        {
            var content = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<Identity>(content);
        }
        /// <summary>
        /// Gets <see cref="Identity"/> from .xml file
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns><see cref="Identity"/></returns>
        public static Identity FromXmlFile(string path)
        {
            Identity result;
            var serializer = new XmlSerializer(typeof(Identity));

            using (var stream = new FileStream(path, FileMode.Open))
            {
                result = (Identity)serializer.Deserialize(stream);
            }

            return result;
        }

        #endregion
    }
}
