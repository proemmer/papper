using System;

namespace Papper
{
    public abstract class DataPack
    {       
        public DateTime Timestamp { get; protected set; }
        public ExecutionResult ExecutionResult { get; protected set; }

        public virtual DataPack ApplyResult(ExecutionResult result)
        {
            ExecutionResult = result;
            return this;
        }
        public abstract DataPack ApplyResult<T>(ExecutionResult result, T value);
    }
}
