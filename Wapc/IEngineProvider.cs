using Wapc;

public interface IEngineProvider
{
    void Init(ModuleState host);

    int Call(int operationLen, int msgLen);
    
    void Replace(byte[] bytes);
}
