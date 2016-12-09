using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForcePush.Output
{
    public class ConsoleWriter : IOutput
    {
        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}
