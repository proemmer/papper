using System;

namespace Papper.Internal
{
    public enum OptimizerType
    {
        Block,
        Items
    }


    internal static class OptimizerFactory
    {
        public static IReadOperationOptimizer CreateOptimizer(OptimizerType type)
        {
            switch(type)
            {
                case OptimizerType.Block:
                    return new BlockBasedReadOperationOptimizer();
                case OptimizerType.Items:
                    return new ItemBasedReadOperationOptimizer();
                default:
                    throw new ArgumentException($"Unknown optimizer type given!");
            }
            
        }
    }
}
