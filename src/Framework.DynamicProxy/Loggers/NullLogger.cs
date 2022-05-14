using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    public class NullLogger: ILogger
    {
        public static readonly NullLogger Instance = new NullLogger();
    }
}
