namespace Wapc;

public class Invocation
{
    public string Operation { get; set; }

    public byte[] Msg { get; set; }

    public Invocation(string operation, byte[] msg)
    {
        Operation = operation;
        Msg = msg;
    }
}
