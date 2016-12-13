using System.Collections.Generic;
using System.Linq;
using ForcePush.Diffing;

namespace ForcePush.Test.Unit.Fakes
{
    public class FakeCmd : ICmd
    {
        public List<string> IssuedCommands { get; set; } = new List<string>();
        public Queue<List<string>> Responses { get; set; } = new Queue<List<string>>();
         
        public List<string> Execute(string command, string workingDirectory = "")
        {
            IssuedCommands.Add(command);
            if(Responses.Any())
            {
                return Responses.Dequeue();
            }

            return new List<string>();
        }

        public FakeCmd AddResponse(string response)
        {
            return AddResponse(new List<string> {response});
        }

        public FakeCmd AddResponse(List<string> responses)
        {
            Responses.Enqueue(responses);
            return this;
        }
    }
}