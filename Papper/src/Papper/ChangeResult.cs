namespace Papper
{
    public struct ChangeResult
    {
        /// <summary>
        /// True if the current read was canceled
        /// </summary>
        public bool IsCanceled { get; }

        /// <summary>
        /// True if is complete
        /// </summary>
        public bool IsCompleted { get; }

        /// <summary>
        /// The <see cref="PlcReadResult"/> that was read
        /// </summary>
        public PlcReadResult[] Results { get; }

        public ChangeResult(PlcReadResult[] result, bool isCancelled, bool isCompleted)
        {
            Results = result;
            IsCanceled = isCancelled;
            IsCompleted = isCompleted;
        }
    }
}
