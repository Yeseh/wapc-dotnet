namespace Wapc;


public class WapcHost 
{
    public delegate byte[] Callback(ulong id, string bd, string ns, string op, byte[] payload);

    // Atomic? wapchost.rs:38
    private static ulong ModuleCount = 0;

    IEngineProvider Engine { get; set; }  

    ModuleState State { get; set; }

    public WapcHost(IEngineProvider engine, Callback hostcb)
    {
        var id = ++ModuleCount;
        Engine = engine;
        State = new ModuleState(hostcb);

        Engine.Init(State);
    }

    public void Replace(byte[] module)
    {
        Engine.Replace(module);
    }

    public byte[] Call(string op, byte[] payload)
    {
        var inv = new Invocation(op, payload);
        var opLen = inv.Operation.Length;
        var msgLen = inv.Msg.Length;

        State.Init(inv);

        try
        {
            var result = Engine.Call(opLen, msgLen);

            // Call failed
            if (result == 0)
            {
                var guestError = State.GetGuestError();
                var hasError = !string.IsNullOrWhiteSpace(guestError);

                if (!hasError)
                {
                    throw new Exception("GuestCallFailure, no error message set for call failure");
                }

                throw new Exception($"GuestCallFalure, {guestError}");

            }
            // Call succeeded
            else
            {
                var guestError = State.GetGuestError();
                var guestResponse = State.GetGuestResponse();

                var hasError = !string.IsNullOrWhiteSpace(guestError);
                var hasResponse = guestResponse.Length > 0 && guestResponse != null;

                // TODO Custom exceptions
                if (!hasError && !hasResponse)
                {
                    throw new Exception("GuestCallFailure, no response or error set");
                }
                else if (hasError)
                {
                    throw new Exception($"GuestCallFailure, {guestError}");
                }
                else
                {
                    return guestResponse!;
                }
            }

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
