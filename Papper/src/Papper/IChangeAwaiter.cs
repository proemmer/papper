using System;
using System.Collections.Generic;
using System.Text;

namespace Papper
{
    public interface IChangeAwaiter
    {
        /// <summary>
        /// This property is true if the read is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// The read result.
        /// </summary>
        /// <returns>The read result as a <see cref="ChangeResult"/>.</returns>
        ChangeResult GetResult();

        /// <summary>
        /// This method will be called to complete a read.
        /// </summary>
        /// <param name="continuation"></param>
        void OnCompleted(Action continuation);
    }
}
