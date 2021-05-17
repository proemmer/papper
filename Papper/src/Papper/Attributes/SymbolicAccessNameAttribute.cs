using System;

namespace Papper.Attributes
{

    /// <summary>
    /// You can generate a data structure from code, and if the variables names not match with the access name, you can redefine the names by adding a SymbolicAccessName on the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SymbolicAccessNameAttribute : Attribute
    {
        public SymbolicAccessNameAttribute(string name) => Name = name;

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