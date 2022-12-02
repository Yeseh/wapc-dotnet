using System.Text;
using Wapc;
using Wapc.Providers.Wasmtime;

var module = File.ReadAllBytes("D:\\Jesse\\repo\\wasm-dotnet\\modules\\compiled\\rust.wasm");
var provider = new Builder()
    .ModuleBytes(module)
    .Build();

var host = new WapcHost(provider, callback);

var payload = Encoding.UTF8.GetBytes("Hello, WAPC!");
host.Call("hello_wapc", payload);

static byte[] callback(ulong id, string bd, string ns, string op, byte[] payload) 
{
    Console.WriteLine(@$"
Received call from guest

Binding: {bd}
Namespace: {ns}
Operation: {op}
Payload: {Encoding.UTF8.GetString(payload)}
");

return Array.Empty<byte>();
}
