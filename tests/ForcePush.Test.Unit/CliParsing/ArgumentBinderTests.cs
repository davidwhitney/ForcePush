﻿using System;
using System.Collections.Generic;
using ForcePush.CliParsing;
using NUnit.Framework;

namespace ForcePush.Test.Unit.CliParsing
{
    [TestFixture]
    public class ArgumentBinderTests
    {
        [Test]
        public void GivenArgs_BindsCorrectly()
        {
            var paramz = new List<string> {"-A=Aa", "-B=Bb"}.ToArray();

            var instance = new ArgumentBinder().Bind<FakeClass>(paramz);

            Assert.That(instance.A, Is.EqualTo("Aa"));
            Assert.That(instance.B, Is.EqualTo("Bb"));
        }

        [Test]
        public void GivenUnsupportedTargetType_Throws()
        {
            var paramz = new List<string> { "-C=123" }.ToArray();

            var ex = Assert.Throws<Exception>(() => new ArgumentBinder().Bind<FakeClass>(paramz));

            Assert.That(ex.Message, Is.EqualTo("The binder only supports string properties. Could not bind '-C=123' to 'Int32'."));
        }

        [Test]
        public void GivenArgs_WithQuotes_BindsCorrectly()
        {
            var paramz = new List<string> { "-A=\"Aa\"", "-B='Bb'" }.ToArray();

            var instance = new ArgumentBinder().Bind<FakeClass>(paramz);

            Assert.That(instance.A, Is.EqualTo("Aa"));
            Assert.That(instance.B, Is.EqualTo("Bb"));
        }

        [Test]
        public void Hint_ProvidesExamples()
        {
            var hint = new ArgumentBinder().Hint<FakeClass>();

            Assert.That(hint[0], Is.EqualTo("Supported arguments:"));
            Assert.That(hint[1], Is.EqualTo("\t\t\t-a=... (string)"));
            Assert.That(hint[2], Is.EqualTo("\t\t\t-b=... (string)"));
            Assert.That(hint[3], Is.EqualTo("\t\t\t-optional=... (string, optional)"));
        }

        public class FakeClass
        {
            public string A { get; set; }
            public string B { get; set; }
            public int C { get; set; }
            [Optional] public string Optional { get; set; }
        }
    }
}