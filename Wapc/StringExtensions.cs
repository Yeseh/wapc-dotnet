using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wapc;

public static class StringExtensions
{
    public static byte[] AsBytes(this string str) 
        => Encoding.UTF8.GetBytes(str);
}
