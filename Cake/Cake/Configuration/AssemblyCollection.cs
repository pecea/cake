namespace Cake.Configuration
{
    using System.Configuration;

    /// <summary>
    /// ConfigurationElementCollection class used in AssembliesSection configuration section.
    /// Contains objects of type AssemblyElement. 
    /// </summary>
    public class AssemblyCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Indexer for the AssemblyElement collection.
        /// </summary>
        /// <param name="index">The index of an element we want to retrieve.</param>
        /// <returns>Element of the collection at specified index.</returns>
        public AssemblyElement this[int index]
        {
            get
            {
                return (AssemblyElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Creates new AssemblyElement object.
        /// </summary>
        /// <returns>New, default values-filled AssemblyElement object.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        /// <summary>
        /// Method used for getting path of the assembly the element is representing.
        /// </summary>
        /// <param name="element">Configuration element we want to inspect.</param>
        /// <returns>Path value of the configuration element passed to this method.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Path;
        }
    }
}