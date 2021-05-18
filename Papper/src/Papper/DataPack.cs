using System;

namespace Papper
{
    public abstract class DataPack
    {       
        public DateTime Timestamp { get; protected set; }
        public ExecutionResult ExecutionResult { get; set; }

        public abstract DataPack ApplyData<T>(T data);
    }
}
