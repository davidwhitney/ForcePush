using System.Collections.Generic;
using ForcePush.Output;

namespace ForcePush.Test.Unit.Fakes
{
    public class FakeOutput : List<string>, IOutput
    {
        public void WriteLine(string s)
        {
            Add(s);
        }
    }
}