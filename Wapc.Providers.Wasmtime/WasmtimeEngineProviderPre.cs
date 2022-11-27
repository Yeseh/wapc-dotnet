using Wasmtime;
using Wapc;

namespace Wapc.Providers.Wasmtime;

internal class WasmtimeEngineProviderPre
{
    Module Module;
    WasiParams? WasiParams;
    Engine Engine;
    Linker Linker;
    InstancePre InstancePre;
    EpochDeadlines? EpochDeadlines;

    internal WasmtimeEngineProviderPre(
        Engine engine, 
        Module module, 
        WasiParams? wasiParams = null)
    {

    }


}
