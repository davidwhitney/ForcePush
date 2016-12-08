using System.Collections.Generic;
using ForcePush.Diffing;
using NUnit.Framework;

namespace ForcePush.Test.Unit.Diffing
{
    [TestFixture]
    public class CmdTests
    {
        [Test]
        public void Execute_GivenCommand_ReturnsResult()
        {
            var cmd = new Cmd();

            var result = cmd.Execute("dir");

            Assert.That(result, Is.TypeOf<List<string>>());
            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}