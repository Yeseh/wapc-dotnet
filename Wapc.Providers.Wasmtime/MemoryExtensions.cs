using Wasmtime;

namespace Wapc.Providers.Wasmtime;

public static class MemoryExtensions
{
    public static void WriteSlice(this Memory mem, long address, byte[] slice)
    {
        var ptr = address;
        for (int i = 0, n = slice.Length; i < n; i++)
        {
            mem.WriteByte(ptr, slice[i]);
            ptr++;
        }
    }

    public static byte[] ReadSlice(this Memory mem, long ptr, int len)
    {
        Span<byte> bytes = stackalloc byte[len];
        for (int i = 0, n = len; i < n; i++)
        {
            bytes[i] = mem.ReadByte(ptr);
            ptr++;
        }

        return bytes.ToArray();
    }
}
