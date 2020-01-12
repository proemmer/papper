using System;

namespace Papper.Attributes
{

    /// <summary>
    /// You can generate a data structure from code, and if the variables names not match with the names in your application, you can redefine the names by adding a AliasName on the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AliasNameAttribute : Attribute
    {
        public AliasNameAttribute(string name) => Name = name;

        /// <summary>
        /// The name to use.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

    }
}