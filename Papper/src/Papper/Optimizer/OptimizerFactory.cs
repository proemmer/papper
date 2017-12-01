using System;
using System.Collections.Generic;
using System.Text;

namespace Papper.Optimizer
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
                    return null;
                default:
                    throw new ArgumentException($"Unknown optimizer type given!");
            }
            
        }
    }
}
