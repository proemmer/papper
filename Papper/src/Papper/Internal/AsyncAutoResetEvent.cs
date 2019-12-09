using System.Collections.Generic;
using System.Threading.Tasks;

namespace Papper.Internal
{
    internal class AsyncAutoResetEvent<T>
    {
        private readonly static Task<T> _completed = Task.FromResult<T>(default!);
        private readonly Queue<TaskCompletionSource<T>> _waits = new Queue<TaskCompletionSource<T>>();
        private bool _signaled;

        public Task<T> WaitAsync()
        {
            lock (_waits)
            {
                if (_signaled)
                {
                    _signaled = false;
                    return _completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource<T>();
                    _waits.Enqueue(tcs);
                    return tcs.Task;
                }
            }
        }
        public void Set(T value)
        {
            TaskCompletionSource<T>? toRelease = null;
            lock (_waits)
            {
                if (_waits.Count > 0)
                    toRelease = _waits.Dequeue();
                else if (!_signaled)
                    _signaled = true;
            }
            if (toRelease != null)
                toRelease.SetResult(value);
        }
    }
}
