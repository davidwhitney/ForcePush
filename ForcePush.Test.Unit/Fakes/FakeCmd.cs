﻿using System.Collections.Generic;

namespace ForcePush.Test.Unit.Fakes
{
    public class FakeCmd : List<string>, ICmd
    {
        public string LastCommand { get; set; }

        public List<string> Execute(string command)
        {
            LastCommand = command;
            return this;
        }
    }
}