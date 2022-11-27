using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wapc;

[StructLayout(LayoutKind.Sequential)]
public struct WasiParams
{
    public string[] ArgV;

    public (string, string)[] MapDirs;

    public (string, string)[] EnvVars;

    public string[] PreopenedDirs;
}


