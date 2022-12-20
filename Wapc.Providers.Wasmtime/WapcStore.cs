using Wasmtime;

namespace Wapc.Providers.Wasmtime;

internal class WapcStore : IDisposable
{
    internal ModuleState? Host;

    internal WapcStore(Engine engine, ModuleState? host)
    {
        Host = host;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
