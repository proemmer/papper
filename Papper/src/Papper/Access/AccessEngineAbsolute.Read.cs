using Papper.Extensions.Notification;
using Papper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal partial class AccessEngineAbsolute
    {

        internal override Task<PlcReadResult[]> ReadAsync(IEnumerable<PlcReadReference> vars, bool doNotConvert)
            => ReadExecutionsAsync(DetermineExecutions(vars), doNotConvert: doNotConvert);

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
            return CreatePlcReadResults(executions, needUpdate, changedAfter, filter, doNotConvert);
        }

        internal override PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions,
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

        internal override PlcReadResult[] ReadVariablesFromExecutionCache(IEnumerable<string> variables, List<Execution>? executions)
        {
            return executions.GroupBy(exec => exec.ExecutionResult) // Group by execution result
                                                  .SelectMany(group => group.SelectMany(g => g.Bindings)
                                                                            .Where(b => variables.Contains(b.Key))
                                                                            .Select(b => new PlcReadResult(b.Key,
                                                                                                            b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                                                            group.Key)
                                                                            )).ToArray();
        }

        internal override PlcReadResult[] CreatePlcReadResults(IEnumerable<Execution> executions, IEnumerable<DataPack> packs)
        {
            return packs.OfType<DataPackAbsolute>().Select(pack => executions.FirstOrDefault(x => pack.Selector == x.PlcRawData.Selector &&
                                                                pack.Offset == x.PlcRawData.Offset &&
                                                                pack.Length == (x.PlcRawData.Size > 0 ? x.PlcRawData.Size : 1))?.ApplyDataPack(pack))
                        .Where(exec => exec != null)
                        .GroupBy(exec => exec!.ExecutionResult) // Group by execution result
                        .Where(res => res.Key == ExecutionResult.Ok) // filter by OK results
                        .SelectMany(group => group.SelectMany(g => g!.Bindings)
                                                    .Select(b => new PlcReadResult(b.Key,
                                                                    b.Value?.ConvertFromRaw(b.Value.RawData.ReadDataCache.Span),
                                                                    group.Key)
                                                    )).ToArray();
        }

        internal Task ReadFromPlcAsync(IEnumerable<DataPack> packs) => _readEventHandler != null ? _readEventHandler.Invoke(packs) : Task.CompletedTask;

        private static bool HasChangesSinceLastRun(Execution exec, DateTime? changedAfter) => changedAfter == null || exec.LastChange > changedAfter;

        private static object? ConvertToResult(PlcObjectBinding binding, bool doNotConvert = false)
        {
            if (doNotConvert)
            {
                return binding.RawData.ReadDataCache.Slice(binding.Offset, binding.Size).ToArray();
            }

            return binding?.ConvertFromRaw(binding.RawData.ReadDataCache.Span);
        }



    }
}
