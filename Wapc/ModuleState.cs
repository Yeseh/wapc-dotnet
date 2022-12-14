namespace Wapc;

public class ModuleState
{
    private static readonly ReaderWriterLock Lock = new();

    private Invocation? GuestRequest = null!;

    private byte[] GuestResponse = Array.Empty<byte>();

    private byte[] HostResponse = Array.Empty<byte>();

    private string? HostError = string.Empty;

    private string? GuestError = string.Empty;

    private WapcHost.Callback? HostCallback = null!;

    public ulong Id;

    public ModuleState(WapcHost.Callback? hostcb = null)
    {
        HostCallback = hostcb;
    }

    public Invocation? GetGuestRequest() 
        => ReadState(m => m.GuestRequest);

    public byte[] GetGuestResponse() 
        => ReadState(m => m.GuestResponse);
    
    public byte[] GetHostResponse() 
        => ReadState(m => m.HostResponse);
    public string GetHostError() 
        => ReadState(m => m.HostError ?? string.Empty);

    public string GetGuestError() 
        => ReadState(m => m.GuestError ?? string.Empty);

    public void SetGuestError(string err)
        => WriteState(m => Interlocked.Exchange(ref GuestError, err));
        
    public void SetHostError(string err)
        => WriteState(m => Interlocked.Exchange(ref HostError, err));

    public void SetGuestResponse(byte[] response)
        => WriteState(m => Interlocked.Exchange(ref GuestResponse, response));

    public int Call(
        string binding,
        string ns,
        string operation,
        byte[] payload)
    {
        WriteState(m =>
        {
            Interlocked.Exchange(ref HostResponse, Array.Empty<byte>());
            Interlocked.Exchange(ref HostError, null);
        });

        if (HostCallback == null)
        {
            throw new ApplicationException("No hostcallback defined");
        }

        try
        {
            var result = HostCallback.Invoke(Id, binding, ns, operation, payload);
            if (result != null)
            {
                // NOTE: This is capturing 'result', keep an eye for allocations;
                WriteState(m =>
                {
                    Interlocked.Exchange(ref HostResponse, result);
                });
            }
            else
            {
                WriteState(m =>
                {
                    Interlocked.Exchange(ref HostError, $"HostCb failed");
                });
            }

            return 0;
       }
        catch (Exception ex)
        {
            // TODO: Handle error
            Console.WriteLine(ex);
            return 1;
        }
    }

    public void ConsoleLog(string message)
    {
        Console.WriteLine($"Guest module {Id}: {message}");
    }

    public void Init(Invocation inv)
    {
        WriteState(m =>
        {
            Interlocked.Exchange(ref GuestRequest, inv);
            Interlocked.Exchange(ref GuestResponse, Array.Empty<byte>());
            Interlocked.Exchange(ref HostResponse, Array.Empty<byte>());

            Interlocked.Exchange(ref GuestError, null);
            Interlocked.Exchange(ref HostError, null);
        });
    }

    private T ReadState<T>(Func<ModuleState, T> wrappedFn, int timeout = 50)
    {
        try
        {
            Lock.AcquireReaderLock(timeout);
            try
            {
                return wrappedFn(this);
            }
            finally
            {
                if (Lock.IsReaderLockHeld)
                {
                    Lock.ReleaseReaderLock();
                }
            }
        }
        catch (ApplicationException ex)
        {
            // TODO: Read lock timeout
            throw;
        }
    }

    private void WriteState(Action<ModuleState> wrappedFn, int timeout = 50)
    {
        try
        {
            Lock.AcquireWriterLock(timeout);
            try
            {
                wrappedFn(this);
            }
            finally
            {
                if (Lock.IsWriterLockHeld)
                {
                    Lock.ReleaseWriterLock();
                }
            }
        }
        catch (ApplicationException ex)
        {
            // TODO: Write lock timeout
            throw;
        }
    }
}
