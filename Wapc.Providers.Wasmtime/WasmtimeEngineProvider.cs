using Wapc;
using Wasmtime;

namespace Wapc.Providers.Wasmtime;

using static WapcConstants;

public class WasmtimeEngineProvider : IEngineProvider
{
    internal delegate int GuestCall(int ptr, int len);

    Module _module;
    WasiParams? _wasiParams;
    Engine _engine;
    Linker _linker;
    Store _store;
    EpochDeadlines? _epochDeadlines;
    ModuleState _host;
    Function _guestCall;
    Instance _instance;

    public WasmtimeEngineProvider(Engine engine, byte[] moduleBytes, WasiParams? wasiParams = null)
    {
        _engine = engine;
        _linker = new Linker(_engine);
        _module = Module.FromBytes(_engine, "module", moduleBytes);
        _host = new();
        _store = new Store(_engine, _host);

        // TODO: Conditional WASI configuration
        _linker.DefineWasi();
        _linker!.ConfigureWapc();

        Init(_host);
    }

    public int Call(int operationLen, int msgLen)
    {
        if (_epochDeadlines.HasValue)
        {
            _store.SetEpochDeadline(_epochDeadlines.Value.WapcFunc);
        }

        try
        {
            var call = (int)_guestCall.Invoke(operationLen, msgLen)!;
            return call;
        }
        catch (Exception e) 
        {
            // TODO: Handle error
            throw;
        }
    }

    public void Init(ModuleState host)
    {
        var intType = typeof(int);
        var wasiConfig = new WasiConfiguration()
            .WithInheritedStandardInput()
            .WithInheritedStandardOutput()
            .WithInheritedStandardError();

        _store = new Store(_engine, host);
        _store.SetWasiConfiguration(wasiConfig);
        _instance = _linker.Instantiate(_store, _module);
        _guestCall = _instance.GetFunction(GUEST_CALL, intType, intType, intType)!;

        if (_guestCall == null) { throw new Exception("guest_call was not defined"); }

        foreach (var starter in REQUIRED_STARTS)
        {
            if (_epochDeadlines.HasValue) 
            {
                _store.SetEpochDeadline(_epochDeadlines.Value.WapcInit);
            }

            var starterFn = _instance.GetFunction(starter);
            try
            {
                if (starterFn != null) { starterFn.Invoke(); }
            }
            catch (Exception ex) 
            {
                // TODO: Handle failed initialization
                throw;
            }
        }
    }

    public void Replace(byte[] bytes)
    {
        throw new NotImplementedException();
    }
}