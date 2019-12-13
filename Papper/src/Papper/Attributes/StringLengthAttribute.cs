using System;

namespace Papper.Attributes
{

    /// <summary>
    /// Validation attribute to assert a string property, field or parameter does not exceed a maximum length
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class StringLengthAttribute : Attribute
    {
        /// <summary>
        /// Gets the maximum acceptable length of the string.
        /// </summary>
        public int MaximumLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the minimum acceptable length of the string.
        /// </summary>
        public int MinimumLength
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor that accepts the maximum length of the string.
        /// </summary>
        /// <param name="maximumLength">The maximum length, inclusive.  It may not be negative.</param>
        public StringLengthAttribute(int maximumLength)
        {
            MaximumLength = maximumLength;
        }
    }
}
