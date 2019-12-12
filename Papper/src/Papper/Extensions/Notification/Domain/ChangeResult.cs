using System.Collections.Generic;

namespace Papper
{
    public struct ChangeResult : System.IEquatable<ChangeResult>
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
        public IEnumerable<PlcReadResult>? Results { get; }

        public ChangeResult(IEnumerable<PlcReadResult>? result, bool isCancelled, bool isCompleted)
        {
            Results = result;
            IsCanceled = isCancelled;
            IsCompleted = isCompleted;
        }

        public override bool Equals(object? obj) => obj is ChangeResult result && 
                                                    IsCanceled == result.IsCanceled && 
                                                    IsCompleted == result.IsCompleted && 
                                                    EqualityComparer<IEnumerable<PlcReadResult>?>.Default.Equals(Results, result.Results);

        public override int GetHashCode()
        {
            var hashCode = -239268687;
            hashCode = hashCode * -1521134295 + IsCanceled.GetHashCode();
            hashCode = hashCode * -1521134295 + IsCompleted.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<PlcReadResult>?>.Default.GetHashCode(Results);
            return hashCode;
        }

        public static bool operator ==(ChangeResult left, ChangeResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ChangeResult left, ChangeResult right)
        {
            return !(left == right);
        }

        public bool Equals(ChangeResult other) =>  other != null &&
                                                    IsCanceled == other.IsCanceled &&
                                                    IsCompleted == other.IsCompleted &&
                                                    EqualityComparer<IEnumerable<PlcReadResult>?>.Default.Equals(Results, other.Results);
    }
}
