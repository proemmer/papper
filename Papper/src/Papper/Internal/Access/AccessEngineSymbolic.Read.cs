using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal partial class AccessEngineSymbolic
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
        {
            if (filter == null)
            {
                filter = (x) => x;
            }
            return executions.Select(exec => needUpdate.TryGetValue(exec, out var pack) ? exec.ApplyDataPack(pack) : exec)
                             .Where(exec => HasChangesSinceLastRun(exec, changedAfter))
                             .GroupBy(exec => exec.ExecutionResult) // Group by execution result
                             .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                             .SelectMany(group => filter(group.SelectMany(g => g.Bindings))
                                                       .Select(b => new PlcReadResult(b.Key,
                                                                                      ConvertToResult(b.Value, doNotConvert),
                                                                                      group.Key)
                                                       )).ToArray();
        }


        internal override PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs) => throw new NotImplementedException();
        
        internal override PlcReadResult[] ReadVariablesFromExecutionCache(IEnumerable<string> variables, List<Execution>? executions) => throw new NotImplementedException();


        private static bool HasChangesSinceLastRun(Execution exec, DateTime? changedAfter) => changedAfter == null || exec.LastChange > changedAfter;

        private static object? ConvertToResult(PlcObjectBinding binding, bool doNotConvert = false)
        {
            if (doNotConvert)
            {
                throw new NotImplementedException();
            }
            return binding.RawData.ReadDataCache;
        }
    }
}
