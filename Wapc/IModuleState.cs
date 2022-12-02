namespace Wapc;

public interface IModuleState
{
    void ConsoleLog(string message);
    void Call(string binding, string ns, string operation, byte[] payload);
}