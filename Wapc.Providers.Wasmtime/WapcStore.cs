using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wapc;
using Wasmtime;

namespace Wapc.Providers.Wasmtime;

internal class WapcStore : IDisposable
{
    internal ModuleState Host;

    internal WapcStore(Engine engine, ModuleState? host)
    {
        Host = host;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
