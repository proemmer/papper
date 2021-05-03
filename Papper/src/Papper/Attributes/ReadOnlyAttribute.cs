//------------------------------------------------------------------------------
// <copyright file="ReadOnlyAttribute.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
//     Modified to use in DNX  from Benjamin Proemmer
// </copyright>                                                                
//------------------------------------------------------------------------------

/*
 */
using System;


namespace Papper.Attributes
{
    /// <devdoc>
    ///    <para>Specifies whether the property this attribute is bound to
    ///       is read-only or read/write.</para>
    /// </devdoc>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ReadOnlyAttribute : Attribute
    {

        /// <devdoc>
        ///    <para>
        ///       Specifies that the property this attribute is bound to is read-only and
        ///       cannot be modified in the server explorer. This <see langword='static '/>field is
        ///       read-only.
        ///    </para>
        /// </devdoc>
        public static readonly ReadOnlyAttribute Yes = new(true);

        /// <devdoc>
        ///    <para>
        ///       Specifies that the property this attribute is bound to is read/write and can
        ///       be modified at design time. This <see langword='static '/>field is read-only.
        ///    </para>
        /// </devdoc>
        public static readonly ReadOnlyAttribute No = new(false);

        /// <devdoc>
        ///    <para>
        ///       Specifies the default value for the <see cref='System.ComponentModel.ReadOnlyAttribute'/> , which is <see cref='System.ComponentModel.ReadOnlyAttribute.No'/>, that is,
        ///       the property this attribute is bound to is read/write. This <see langword='static'/> field is read-only.
        ///    </para>
        /// </devdoc>
        public static readonly ReadOnlyAttribute Default = No;

        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='System.ComponentModel.ReadOnlyAttribute'/> class.
        ///    </para>
        /// </devdoc>
        public ReadOnlyAttribute(bool isReadOnly) => IsReadOnly = isReadOnly;

        /// <devdoc>
        ///    <para>
        ///       Gets a value indicating whether the property this attribute is bound to is
        ///       read-only.
        ///    </para>
        /// </devdoc>
        public bool IsReadOnly { get; } = false;

        /// <internalonly/>
        /// <devdoc>
        /// </devdoc>
        public override bool Equals(object value)
        {
            if (this == value)
            {
                return true;
            }

            return value is ReadOnlyAttribute other && other.IsReadOnly == IsReadOnly;
        }

        /// <devdoc>
        ///    <para>
        ///       Returns the hashcode for this object.
        ///    </para>
        /// </devdoc>
        public override int GetHashCode() => base.GetHashCode();

    }
}
