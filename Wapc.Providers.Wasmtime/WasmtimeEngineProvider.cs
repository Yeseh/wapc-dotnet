using Wapc;
using Wasmtime;

namespace Wapc.Providers.Wasmtime;

public class WasmtimeEngineProvider : IEngineProvider
{
    Module Module;
    WasiParams? WasiParams;
    EngineInner? Inner;
    Engine Engine;
    Linker Linker;
    Store Store;
    InstancePre InstancePre;
    EpochDeadlines? EpochDeadlines;
    ModuleState ModuleState;

    public WasmtimeEngineProvider(byte[] moduleBytes, WasiParams? wasiParams = null)
    {
        Engine = new Engine();
        Linker = new Linker(Engine);
        Store = new Store(Engine);
        ModuleState = new();
        Module = Module.FromBytes(Engine, "module", moduleBytes);
        

        if (wasiParams != null)
        {
            Linker.DefineWasi();
        }

        Linker!.AddWapcCallbacks(Host);
    }

    public int Call(int operationLen, int msgLen)
    {
        if (EpochDeadlines.HasValue)
        {
            Store.SetEpochDeadline(EpochDeadlines.Value.WapcFunc);
        }
        


        throw new NotImplementedException();
    }

    public void Init(ModuleState host)
    {
        throw new NotImplementedException();
    }

    public void Replace(byte[] bytes)
    {
        throw new NotImplementedException();
    }
}