using System;
using System.Collections.Generic;

namespace Papper.Internal
{
    //Use: new LambdaComparer<LogFileConfigurator>((p1, p2) => p1.MachineName == p2.MachineName)).
    //     new LambdaComparer<LogFileConfigurator>((p1, p2) => p1.MachineName.Length.CompareTo(p2.MachineName.Length))).
    internal class LambdaComparer<T> : IComparer<T>, IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _lambdaEquals;
        private readonly Func<T, T, int> _lambdaCompare;
        private readonly Func<T, int> _lambdaHash;

        public LambdaComparer(Func<T, T, int> lambdaComparer) :
            this(lambdaComparer, (x, y) => lambdaComparer != null && lambdaComparer(x, y) == 0, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> lambdaEquals) :
            this((x, y) => 0, lambdaEquals, o => 0)
        {
        }

        public LambdaComparer(Func<T, T, bool> lambdaEquals, Func<T, int> lambdaHash) :
            this((x, y) => 0, lambdaEquals, lambdaHash)
        {
        }

        public LambdaComparer(Func<T, T, int> lambdaComparer, Func<T, int> lambdaHash) :
            this((x, y) => 0, (x, y) => lambdaComparer != null && lambdaComparer(x, y) == 0, lambdaHash)
        {
        }

        public LambdaComparer(Func<T, T, int> lambdaCompare, Func<T, T, bool> lambdaEquals, Func<T, int> lambdaHash)
        {
            _lambdaCompare = lambdaCompare ?? throw new ArgumentNullException("lambdaCompare");
            _lambdaEquals = lambdaEquals ?? throw new ArgumentNullException("lambdaEquals");
            _lambdaHash = lambdaHash ?? throw new ArgumentNullException("lambdaHash");
        }

        public int Compare(T x, T y)
        {
            return _lambdaCompare(x, y);
        }

        public bool Equals(T x, T y)
        {
            return _lambdaEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _lambdaHash(obj);
        }
    }
}
