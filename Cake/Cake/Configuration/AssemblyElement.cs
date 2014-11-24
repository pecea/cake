﻿namespace Cake.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// ConfigurationElement class defining a configuration file entry describing an assembly we want to include in execution of a script.
    /// </summary>
    public class AssemblyElement : ConfigurationElement
    {
        /// <summary>
        /// Configuration element property defining path of the assembly we want to load.
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (String)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}