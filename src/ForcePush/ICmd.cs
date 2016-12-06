using System.Collections.Generic;

namespace ForcePush
{
    public interface ICmd
    {
        List<string> Execute(string command);
    }
}