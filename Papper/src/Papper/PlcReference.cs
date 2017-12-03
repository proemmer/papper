using System.Collections.Generic;

namespace Papper
{
    public class PlcReference
    {
        public string Mapping { get; set; }
        public string[] Variables { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"> [Mapping].[Variable]</param>
        /// <returns></returns>
        public static PlcReference FromAddress(string address)
        {
            var firstDot = address.IndexOf('.');
            return new PlcReference
            {
                Mapping = address.Substring(0, firstDot),
                Variables = new string[] { address.Substring(firstDot + 1) }
            };
        }

        public static IEnumerable<PlcReference> FromRoot(string root, params string[] variables)
        {
            foreach (var variable in variables)
            {
                yield return PlcReference.FromAddress($"{root}.{variable}");
            }
        }
    }
}