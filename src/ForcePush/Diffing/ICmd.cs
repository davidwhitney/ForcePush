using System.Collections.Generic;

namespace ForcePush.Diffing
{
    public interface ICmd
    {
        List<string> Execute(string command);
    }
}