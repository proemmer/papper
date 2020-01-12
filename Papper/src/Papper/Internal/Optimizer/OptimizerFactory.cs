namespace Papper.Internal
{
    internal static class OptimizerFactory
    {
        public static IReadOperationOptimizer CreateOptimizer(OptimizerType type)
        {
            switch (type)
            {
                case OptimizerType.Block:
                    return new BlockBasedReadOperationOptimizer();
                case OptimizerType.Items:
                    return new ItemBasedReadOperationOptimizer();
                default:
                    ExceptionThrowHelper.ThrowUnknownOptimizrException(type);
                    return new ItemBasedReadOperationOptimizer(); // will not be called because of exception throwing
            }

        }
    }
}
