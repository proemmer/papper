namespace Papper
{
    public struct PlcWriteResult
    {
        private string _address;
        private ExecutionResult _executionResult;
        private int _dot;

        public PlcWriteResult(string address, ExecutionResult executionResult)
        {
            _address = address;
            _executionResult = executionResult;
            _dot = address.IndexOf(".");
        }

        public string Address => _address;

        public ExecutionResult ActionResult => _executionResult;

        public string Mapping => Address.Substring(0, _dot);

        public string Variable => Address.Substring(_dot + 1);

    }
}