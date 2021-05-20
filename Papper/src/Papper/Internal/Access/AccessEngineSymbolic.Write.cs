using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Papper.Access
{
    internal partial class AccessEngineSymbolic
    {
        internal override Task<PlcWriteResult[]> WriteAsync(IEnumerable<PlcWriteReference> vars) => throw new NotImplementedException();
    }
}
