namespace Papper.Extensions.Metadata
{
    public class MetaDataPack
    {

        public string MappingName { get; private set; }
        public string AbsoluteName { get; private set; }
        public BlockMetaData? MetaData { get; set; }


        public ExecutionResult ExecutionResult { get; set; }

        public MetaDataPack(string mappingName, string absoluteName)
        {
            MappingName = mappingName ?? ExceptionThrowHelper.ThrowArgumentNullException<string>(nameof(mappingName));
            AbsoluteName = absoluteName ?? ExceptionThrowHelper.ThrowArgumentNullException<string>(nameof(mappingName));
        }
    }
}
