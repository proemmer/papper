using System.Collections.Generic;

namespace Papper
{
    public struct PlcReadReference : IPlcReference
    {
        public string Mapping { get; internal set; }
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


        public static IEnumerable<PlcReadReference> FromRoot(string root, params string[] variables)
        {
            foreach (var variable in variables)
            {
                yield return FromAddress($"{root}.{variable}");
            }
        }
    }
}