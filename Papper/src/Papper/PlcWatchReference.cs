using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define read operations.
    /// </summary>
    public struct PlcWatchReference : IPlcReference
    {
        private readonly int _dot;


        /// mapping part of the address
        /// </summary>
        public string Mapping => Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => Address.Substring(_dot + 1);


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
        /// Returns the watch reference as a read reference
        /// </summary>
        /// <returns></returns>
        public PlcReadReference ToReadReference() => new PlcReadReference(Address);


        public PlcWatchReference(string address, int watchCycle = 1000)
        {
            Address = address;
            WatchCycle = watchCycle;
            _dot = address.IndexOf(".");
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
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}", watchCycle);
            }
        }
    }
}