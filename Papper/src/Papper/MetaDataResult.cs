namespace Papper
{
    public class MetaDataResult
    {


        public MetaData MetaData { get; private set; }
        public ExecutionResult ExecutionResult { get; private set; }

        public MetaDataResult(MetaData  resultData, ExecutionResult result)
        {
            MetaData = resultData;
            ExecutionResult = result;
        }
    }
}
