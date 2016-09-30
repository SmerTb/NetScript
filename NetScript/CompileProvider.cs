using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace NetScript
{
    public enum CompileProvider : byte
    {
        SharpCompiller,
        VisualBasicCompiller
    }
}
