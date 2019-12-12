namespace Papper.Extensions.Metadata
{
    public class MetaDataResult
    {


        public BlockMetaData? MetaData { get; private set; }
        public ExecutionResult ActionResult { get; private set; }

        public MetaDataResult(BlockMetaData?  resultData, ExecutionResult result)
        {
            MetaData = resultData;
            ActionResult = result;
        }
    }
}
