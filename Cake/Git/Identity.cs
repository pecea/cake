using System;
using System.IO;
using System.Xml.Serialization;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Newtonsoft.Json;
using Git.Helpers;

namespace Git
{
    public class Identity
    {
        private string _name;
        private string _email;
        private string _username;
        private string _password;

        public string Name
        {
            get => (_name = AskIfNull(_name, nameof(Name)));
            set => _name = value;
        }

        public string Email
        {
            get => (_email = AskIfNull(_email, nameof(Email)));
            set => _email = value;
        }

        public string Username
        {
            get => (_username = AskIfNull(_username, nameof(Username)));
            set => _username = value;
        }

        public string Password
        {
            get => (_password = AskIfNull(_password, nameof(Password), masked: true));
            set => _password = value;
        }

        private string AskIfNull(string value, string name, bool masked = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine($"Please provide your git {name}:");

                if (masked)
                    return ConsoleHelper.ReadLineMasked();
                return Console.ReadLine();
            }

            return value;
        }

        public CredentialsHandler CredentialsProvider => GetCredentials;

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

        public static Identity FromJsonFile(string path)
        {
            var content = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<Identity>(content);
        }

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
