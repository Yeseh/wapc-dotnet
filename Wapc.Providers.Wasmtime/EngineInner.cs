using Wasmtime;

namespace Wapc.Providers.Wasmtime;

internal class EngineInner
{
    public static readonly ReaderWriterLock Lock = new();

    internal Instance? Instance;

    internal ModuleState? Host;
}