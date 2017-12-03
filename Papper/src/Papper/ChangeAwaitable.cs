using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Papper
{
    public struct ChangeAwaitable : INotifyCompletion
    {
        private readonly IChangeAwaiter _awaiter;

        public ChangeAwaitable(IChangeAwaiter awaiter)
        {
            _awaiter = awaiter;
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public ChangeResult GetResult() => _awaiter.GetResult();

        public ChangeAwaitable GetAwaiter() => this;

        public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);
    }
}
