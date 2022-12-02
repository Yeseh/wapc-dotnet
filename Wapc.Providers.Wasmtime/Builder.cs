using Wasmtime;

namespace Wapc.Providers.Wasmtime;

// TODO: This class makes more sense in the Rust implementation, can probably ditch it
public class Builder 
{
    Engine? _engine;

    Module? _module;

    byte[]? _moduleBytes;

    // TODO: Cache
    // bool _cacheEnabled
    // path _cachePath

    WasiParams? _wasiParams;

    EpochDeadlines? _epochDeadlines;

    public Builder() { }

    public Builder ModuleBytes(byte[] moduleBytes)
    {
        this._moduleBytes = moduleBytes;
        return this;
    }

    public Builder Module(Module module)
    {
        this._module = module;
        return this;
    }

    public Builder Engine(Engine engine)
    {
        this._engine = engine;
        return this;
    }

    public Builder WasiParams(WasiParams wasiParams) 
    {
        this._wasiParams = wasiParams;
        return this;
    }
    
    public Builder EnableEpochInterruptions(ulong initDeadline, ulong funcDeadline) 
    {
        this._epochDeadlines = new()
        {
            WapcFunc = funcDeadline,
            WapcInit = initDeadline
        };
        return this;
    }

    public WasmtimeEngineProvider Build()
    {
        if (_moduleBytes == null)
        {
            throw new Exception("Module bytes is required");
        }

        if (_engine == null) 
        {
            _engine = new();
        }

        return new(_engine, _moduleBytes!, _wasiParams);
    }
}
