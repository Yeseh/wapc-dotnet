using System.Text;
using Wasmtime;

using static Wapc.WapcConstants;

namespace Wapc.Providers.Wasmtime;

internal static class LinkerExtensions
{
    // TODO: Closing over host, does that work?
    // https://github.com/bytecodealliance/wasmtime-dotnet/issues/190
    public static void AddWapcCallbacks(this Linker linker, ModuleState host)
    {
        //linker.DefineFunction(
        //    HOST_NAMESPACE,
        //    GUEST_REQUEST,
        //    (Caller caller, int opPtr, int ptr) =>
        //    {
        //        var inv = host.GetGuestRequest();
        //        var mem = caller.GetMemory("memory");

        //        if (inv != null && mem != null)
        //        {
        //            for (int i = 0; i < inv.Msg.Length; i++)
        //            {
        //                var address = ptr + i;
        //                mem.WriteByte(address, inv.Msg[i]);
        //            }

        //            var opSpan = inv.Operation.AsSpan();
        //            for (int i = 0; i < opSpan.Length; i++)
        //            {
        //                var address = opPtr + i;
        //                mem.WriteByte(address, (byte)opSpan[i]);
        //            }
        //        }

        //        // TODO catch errors
        //    }
        //);
        
        linker.DefineFunction(
            HOST_NAMESPACE,
            HOST_CONSOLE_LOG,
            (Caller caller, int ptr, int len) =>
            {
                var inv = host.GetGuestRequest();
                var mem = caller.GetMemory("memory");

                if (mem != null)
                {
                    var str = mem.ReadString(ptr, len, Encoding.UTF8); 
                    host.ConsoleLog(str);
                }

                // TODO catch errors
            }
        );

        // TODO: Other callbacks
    }
}
