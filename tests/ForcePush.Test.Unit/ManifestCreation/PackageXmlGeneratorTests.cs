using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using ForcePush.ManifestCreation;
using Humanizer;
using NUnit.Framework;

namespace ForcePush.Test.Unit.ManifestCreation
{
    [TestFixture]
    public class PackageXmlGeneratorTests
    {
        private PackageXmlGenerator _gen;
        private MockFileSystem _fakeFilesystem;

        [SetUp]
        public void SetUp()
        {
            _fakeFilesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"c:\repo\appMenus\Salesforce1.appMenu", new MockFileData("file1")},
            });

            _gen = new PackageXmlGenerator(_fakeFilesystem);
        }

        [Test]
        public void GenerateFor_GivenDirectory_GeneratesCorrectVersion()
        {
            var package = _gen.GenerateFor("c:\\repo");

            Assert.That(package.version, Is.EqualTo(36.0));
        }

        [Test]
        public void GenerateFor_GivenDirectory_GeneratesAppropriateNodeNames()
        {
            var package = _gen.GenerateFor("c:\\repo");

            Assert.That(package.types[0].name, Is.EqualTo("AppMenu"));
        }

        [Test]
        public void GenerateFor_GivenDirectory_GeneratesAppropriateWildcard()
        {
            var package = _gen.GenerateFor("c:\\repo");

            Assert.That(package.types[0].members[0], Is.EqualTo("*"));
        }

        [TestCase("classes", "ApexClass")]
        [TestCase("components", "ApexComponent")]
        [TestCase("pages", "ApexPage")]
        [TestCase("triggers", "ApexTrigger")]
        public void GenerateFor_GivenDirectoryWithSpecialDirectory_PrefixesWithApex(string dirname, string className)
        {
            _gen = new PackageXmlGenerator(new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {$@"c:\repo\{dirname}\Salesforce1.appMenu", new MockFileData("file1")},
            }));

            var package = _gen.GenerateFor("c:\\repo");

            Assert.That(package.types[0].name, Is.EqualTo(className));
        }

        [Test]
        public void Serialize_GivenPackage_WritesToXml()
        {
            var package = new Package
            {
                types = new[] {new PackageTypes {name = "test", members = new[] {"*"}}},
                version = 36.0m
            };

            var xml = _gen.Serialize(package);

            Assert.That(xml, Is.EqualTo(@"<?xml version=""1.0"" encoding=""utf-16""?>
<Package xmlns=""http://soap.sforce.com/2006/04/metadata"">
  <types>
    <members>*</members>
    <name>test</name>
  </types>
  <version>36.0</version>
</Package>"));
        }
    }
}
