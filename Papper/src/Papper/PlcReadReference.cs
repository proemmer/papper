using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define read operations.
    /// </summary>
    public struct PlcReadReference : IPlcReference, System.IEquatable<PlcReadReference>
    {
        private readonly int _dot;

        /// <summary>
        /// mapping part of the address
        /// </summary>
        public string Mapping => _dot == -1 ? Address : Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => _dot == -1 ? string.Empty : Address.Substring(_dot + 1);


        /// <summary>
        /// Full address is composed of mapping and variable
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcReadReference FromAddress(string address) => new(address);


        public PlcReadReference(string address)
        {
            Address = address;
            _dot = address == null ? -1 : address.IndexOf(".", System.StringComparison.InvariantCulture);
        }


        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Root part of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, params string[] variables) => FromRoot(root, variables as IEnumerable<string>);

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Root part of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, IEnumerable<string> variables)
        {
            if (variables == null)
            {
                yield break;
            }

            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}");
            }
        }

        public override bool Equals(object? obj) => obj is PlcReadReference reference && Mapping == reference.Mapping && Variable == reference.Variable && Address == reference.Address;

        public override int GetHashCode()
        {
            var hashCode = 1304634581;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Mapping);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Variable);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            return hashCode;
        }

        public static bool operator ==(PlcReadReference left, PlcReadReference right) => left.Equals(right);

        public static bool operator !=(PlcReadReference left, PlcReadReference right) => !(left == right);

        public bool Equals(PlcReadReference other) => other != null && Mapping == other.Mapping && Variable == other.Variable && Address == other.Address;
    }
}