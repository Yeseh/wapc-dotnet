namespace Wapc;

public interface IModuleHost
{
    Invocation? GetGuestRequest();
    byte[]? GetHostResponse();
    void SetGuestError(string err);
    void SetGuestResponse(byte[] response);
    string? GetHostError();
    int CallHost(string binding, string ns, string operation, byte[] payload);
    void ConsoleLog(string msg);
}
