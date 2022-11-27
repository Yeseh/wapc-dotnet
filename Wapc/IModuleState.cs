namespace Wapc;

public interface IModuleState
{
    void ConsoleLog(string message);
    void HostCall(string binding, string ns, string operation, byte[] payload);
}