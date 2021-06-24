using Papper.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal partial class AccessEngineSymbolic
    {

        /// <summary>
        /// Write values to variables of an given mapping
        /// </summary>
        /// <param name="mapping">mapping name specified in the MappingAttribute</param>
        /// <param name="values">variable names and values to write</param>
        /// <returns>return true if all operations are succeeded</returns>
        internal override async Task<PlcWriteResult[]> WriteAsync(IEnumerable<PlcWriteReference> vars)
        {
            var values = vars.ToDictionary(x => x.Address, x => x.Value);
            // determine executions
            var executions = DetermineExecutions(vars);

            var prepared = executions.Select(execution => execution.CreateWriteDataPack(values, null))
                                        .ToDictionary(x => x.Key, x => x.Value);

            await WriteToPlcAsync(prepared.Values.SelectMany(x => x)).ConfigureAwait(false);
            executions.ForEach(exec =>
            {
                if (exec.ExecutionResult == ExecutionResult.Ok)
                {
                    exec.Invalidate();
                }
            });

            return CreatePlcWriteResults(values, prepared);
        }



        private static PlcWriteResult[] CreatePlcWriteResults(Dictionary<string, object> values, Dictionary<Execution, IEnumerable<DataPack>> prepared)
        {
            var results = new PlcWriteResult[values.Count];
            var index = 0;
            foreach (var value in values)
            {
                var writePackages = prepared.Where(p => p.Key.Bindings.ContainsKey(value.Key)).SelectMany(x => x.Value);
                var execResult = writePackages.Any() ? ExecutionResult.Ok : ExecutionResult.Unknown;
                foreach (var package in writePackages)
                {
                    if (package.ExecutionResult != ExecutionResult.Ok)
                    {
                        execResult = package.ExecutionResult;
                        break;
                    }
                }
                results[index++] = new PlcWriteResult(value.Key, execResult);
            }

            return results;
        }
    }
}
