﻿namespace Papper
{
    public enum ExecutionResult
    {
        Unknown,  // No result
        Ok,       // plc read/write was ok
        Error     // could not read from/ write to plc
    }
}
