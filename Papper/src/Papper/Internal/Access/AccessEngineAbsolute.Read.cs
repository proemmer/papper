using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal partial class AccessEngineAbsolute
    {

        internal override async Task<PlcReadResult[]> ReadExecutionsAsync(List<Execution> executions,
                                                                          bool onlyOutDated = true,
                                                                          DateTime? changedAfter = null,
                                                                          Func<IEnumerable<string>, DateTime, bool>? forceUpdate = null,
                                                                          Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                                          bool doNotConvert = false)
        {
            // determine outdated
            var needUpdate = UpdateableItems(executions, onlyOutDated, forceUpdate);  // true = read some items from cache!!

            // read from plc
            await ReadFromPlcAsync(needUpdate.Values).ConfigureAwait(false);

            // transform to result
            return ConvertExecutionsToReadResults(executions, needUpdate, changedAfter, filter, doNotConvert);
        }

        internal override PlcReadResult[] ConvertExecutionsToReadResults(IEnumerable<Execution> executions,
                                                              Dictionary<Execution, DataPack> needUpdate,
                                                              DateTime? changedAfter = null,
                                                              Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                              bool doNotConvert = false)
            => ConvertExecutionsToReadResults(executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                                                            .Where(exec => HasChangesSinceLastRun(exec, changedAfter)), filter, doNotConvert);


        internal override PlcReadResult[] ReadVariablesFromExecutionCache(IEnumerable<string> variables, List<Execution> executions) 
            => ConvertExecutionsToReadResults(executions, b => b.Where(x => variables.Contains(x.Key)));

        internal override PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs)
            => ConvertExecutionsToReadResults(packs.OfType<DataPackAbsolute>()
                                                       .Select(pack => executions.FirstOrDefault(x => pack.Selector == x.PlcRawData.Selector &&
                                                                pack.Offset == x.PlcRawData.Offset &&
                                                                pack.Length == (x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1))?.ApplyDataPack(pack)));
        

        private static PlcReadResult[] ConvertExecutionsToReadResults(IEnumerable<Execution?> executions,
                                                              Func<IEnumerable<KeyValuePair<string, PlcObjectBinding>>, IEnumerable<KeyValuePair<string, PlcObjectBinding>>>? filter = null,
                                                              bool doNotConvert = false)
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Where(exec => exec != null)
                             .GroupBy(exec => exec!.ExecutionResult) // Group by execution result
                             .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                             .SelectMany(group => filter(group.SelectMany(g => g!.Bindings))
                                                       .Select(b => new PlcReadResult(b.Key,
                                                                                      ConvertToResult(b.Value, doNotConvert),
                                                                                      group.Key)
                                                       )).ToArray();
        }

        private static bool HasChangesSinceLastRun(Execution exec, DateTime? changedAfter) => changedAfter == null || exec.LastChange > changedAfter;

        private static object? ConvertToResult(PlcObjectBinding binding, bool doNotConvert = false)
        {
            if (binding.RawData.ReadDataCache is Memory<byte> mem)
            {
                if (doNotConvert)
                {
                    return mem.Slice(binding.Offset, binding.Size).ToArray();
                }

                return binding?.ConvertFromRaw(mem.Span);
            }
            return null;
        }



    }
}
