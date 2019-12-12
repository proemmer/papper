using System.Collections.Generic;

namespace Papper.Extensions.Notification
{
    /// <summary>
    /// This class is used to define read operations.
    /// </summary>
    public struct PlcWatchReference : IPlcReference, System.IEquatable<PlcWatchReference>
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
        /// Maximum time between two reads
        /// </summary>
        public int WatchCycle { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcWatchReference FromAddress(string address, int watchCycle) => new PlcWatchReference(address, watchCycle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcWatchReference FromPlcReadReference(PlcReadReference reference, int watchCycle) => new PlcWatchReference(reference.Address, watchCycle);

        /// <summary>
        /// Returns the watch reference as a read reference
        /// </summary>
        /// <returns></returns>
        public PlcReadReference ToReadReference() => new PlcReadReference(Address);


        public PlcWatchReference(string address, int watchCycle = 1000)
        {
            Address = address;
            WatchCycle = watchCycle;
            _dot = address == null ? -1 : address.IndexOf(".", System.StringComparison.InvariantCulture);
        }


        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcWatchReference> FromRoot(string root, int watchCycle, params string[] variables)
            => FromRoot(root, variables as IEnumerable<string>, watchCycle);

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcWatchReference> FromRoot(string root, IEnumerable<string> variables, int watchCycle)
        {
            if (variables == null)
            {
                yield break;
            }

            foreach (string variable in variables)
            {
                yield return FromAddress($"{root}.{variable}", watchCycle);
            }
        }

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcWatchReference> FromRoot(string root, params (string, int)[] variables)
            => FromRoot(root, variables as IEnumerable<(string, int)>);

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcWatchReference> FromRoot(string root, IEnumerable<(string variable, int watchCycle)> variables)
        {
            if (variables == null)
            {
                yield break;
            }

            foreach ((string variable, int watchCycle) variable in variables)
            {
                yield return FromAddress($"{root}.{variable.variable}", variable.watchCycle);
            }
        }

        public override bool Equals(object? obj) => obj is PlcWatchReference reference &&
                                                    Mapping == reference.Mapping &&
                                                    Variable == reference.Variable &&
                                                    Address == reference.Address &&
                                                    WatchCycle == reference.WatchCycle;

        public bool Equals(PlcWatchReference other) => other != null &&
                                                    Mapping == other.Mapping &&
                                                    Variable == other.Variable &&
                                                    Address == other.Address &&
                                                    WatchCycle == other.WatchCycle;

        public override int GetHashCode()
        {
            var hashCode = -1177666424;
            hashCode = hashCode * -1521134295 + _dot.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Mapping);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Variable);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + WatchCycle.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PlcWatchReference left, PlcWatchReference right) => left.Equals(right);

        public static bool operator !=(PlcWatchReference left, PlcWatchReference right) => !(left == right);

    }
}