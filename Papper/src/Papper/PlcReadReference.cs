using System.Collections.Generic;

namespace Papper
{
    /// <summary>
    /// This class is used to define read operations.
    /// </summary>
    public struct PlcReadReference : IPlcReference
    {
        /// <summary>
        /// Mapping part of the address
        /// </summary>
        public string Mapping { get; internal set; }

        /// <summary>
        /// Variable part of the address
        /// </summary>
        public string Variable { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcReadReference FromAddress(string address)
        {
            var firstDot = address.IndexOf('.');
            var area = address.Substring(0, firstDot);
            return new PlcReadReference
            {
                Mapping = area,
                Variable = address.Substring(firstDot + 1)
            };
        }


        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, params string[] variables) => FromRoot(root, variables as IEnumerable<string>);

        /// <summary>
        /// Create a couple of <see cref="PlcReadReference"/> by a given variable root, and some sub variables of the root.
        /// This method can used if you will read more than one variable of the same data block.
        /// </summary>
        /// <param name="root">Rootpart of a variable</param>
        /// <param name="variables">variables</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PlcReadReference"/></returns>
        public static IEnumerable<PlcReadReference> FromRoot(string root, IEnumerable<string> variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}");
            }
        }
    }
}