namespace Cake.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Defines a section used in App.config for adds assembly files to the project.
    /// </summary>
    public class AssembliesSection : ConfigurationSection
    {
        /// <summary>
        /// Defines assemblies element, containing particular assemblies' info.
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