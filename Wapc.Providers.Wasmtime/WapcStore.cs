using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasmtime;

namespace Wapc.Providers.Wasmtime;

internal class WapcStore : Store
{
    internal ModuleState Host;

    internal WapcStore(Engine engine, ModuleState host) : base(engine)
    {
        Host = host;
    }
}
