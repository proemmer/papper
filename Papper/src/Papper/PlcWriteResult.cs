namespace Papper
{
    /// <summary>
    /// This structure represents the result of a write command. It includes the written meta data and 
    /// the <see cref="ExecutionResult"/> of the write operation.
    /// </summary>
    public struct PlcWriteResult
    {
        private readonly int _dot;


        /// <summary>
        /// Full address is composed of mapping and variable
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// The result of an Executed action
        /// </summary>
        public ExecutionResult ActionResult { get; }

        /// <summary>
        /// mapping part of the address
        /// </summary>
        public string Mapping => _dot == -1 ? Address : Address.Substring(0, _dot);

        /// <summary>
        /// variable part of the address.
        /// </summary>
        public string Variable => _dot == -1 ? string.Empty : Address.Substring(_dot + 1);


        /// <summary>
        /// Create an instance of a PlcWriteResult
        /// </summary>
        /// <param name="address">The address of a variable. e.g. [Mapping].[Variable]</param>
        /// <param name="executionResult">The result itself. <see cref="ExecutionResult"/></param>
        public PlcWriteResult(string address, ExecutionResult executionResult)
        {
            Address = address;
            ActionResult = executionResult;
            _dot = address == null ? -1 : address.IndexOf(".", System.StringComparison.InvariantCulture);
        }

    }
}