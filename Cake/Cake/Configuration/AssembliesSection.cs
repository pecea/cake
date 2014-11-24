namespace Cake.Configuration
{
    using System.Configuration;

    /// <summary>
    /// ConfigurationSection class defining a section used in App.config for adding assembly files to the project.
    /// </summary>
    public class AssembliesSection : ConfigurationSection
    {
        /// <summary>
        /// Property defining assemblies element, containing particular assemblies' info.
        /// </summary>
        [ConfigurationProperty("assemblies", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AssemblyCollection))]
        public AssemblyCollection Assemblies
        {
            get
            {
                return (AssemblyCollection)base["assemblies"];
            }
        }
    }
}