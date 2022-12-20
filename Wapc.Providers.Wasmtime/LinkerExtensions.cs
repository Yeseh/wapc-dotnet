using System.Text;
using Wasmtime;

using static Wapc.WapcConstants;

namespace Wapc.Providers.Wasmtime;

internal static class LinkerExtensions
{
    public static void ConfigureWapc(this Linker linker)
    {
        linker.DefineFunction(
            HOST_NAMESPACE,
            GUEST_REQUEST,
            (Caller caller, int op_ptr, int ptr) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;
                var inv = host!.GetGuestRequest();

                if (mem is null)
                {
                    throw new Exception("Memory is null");
                }

                mem.WriteSlice(ptr, inv!.Msg);
                mem.WriteSlice(op_ptr, inv!.Operation.AsBytes());
            });

        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_CONSOLE_LOG,
            (Caller caller, int ptr, int len) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;

                if (mem != null)
                {
                    var str = mem.ReadString(ptr, len, Encoding.UTF8); 
                    host!.ConsoleLog(str);
                }
            }
        );

        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_CALL,
            (Caller caller, int bdPtr, int bdLen, int nsPtr, int nsLen, int opPtr, int opLen, int ptr, int len) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;

                if (mem == null) { throw new Exception("Memory is null"); }

                var payload = mem.ReadSlice(ptr, len);
                var binding = mem.ReadString(bdPtr, bdLen, Encoding.UTF8);
                var nmspace = mem.ReadString(nsPtr, nsLen, Encoding.UTF8);
                var operation = mem.ReadString(nsPtr, nsLen, Encoding.UTF8);

                var result = host.Call(binding, nmspace, operation, payload);

                return result;
            }
        );

        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_RESPONSE,
            (Caller caller, int ptr) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;

                var hostResponse = host!.GetHostResponse();
                mem.WriteSlice(ptr, hostResponse);
            }
        );
        
        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_RESPONSE_LEN,
            (Caller caller) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;
                var hostResponse = host!.GetHostResponse();

                return hostResponse.Length;
            }
        );


        linker.DefineFunction(
            HOST_NAMESPACE,
            GUEST_RESPONSE,
            (Caller caller, int ptr, int len) =>
            {
                var mem = caller.GetMemory("memory");
                var host = caller.GetData() as ModuleState;

                var payload = mem.ReadSlice(ptr, len);
                host.SetGuestResponse(payload);
            }
        );

        linker.DefineFunction(
            HOST_NAMESPACE,
            GUEST_ERROR,
            (Caller caller, int ptr, int len) =>
            {
                var host = caller.GetData() as ModuleState;
                var mem = caller.GetMemory("memory");

                var bytes = mem.ReadSlice(ptr, len);
                var errorStr = Encoding.UTF8.GetString(bytes);
                host.SetGuestError(errorStr);
            }
        );
        
        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_ERROR,
            (Caller caller, int ptr) =>
            {
                var host = caller.GetData() as ModuleState;
                var mem = caller.GetMemory("memory");

                var hostError = host.GetHostError();
                mem.WriteSlice(ptr, hostError.AsBytes());
            }
        );

        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_ERROR_LEN,
            (Caller caller) =>
            {
                var host = caller.GetData() as ModuleState;
                var hostError = host.GetHostError();

                return hostError.Length;
            }
        );
    }
}
