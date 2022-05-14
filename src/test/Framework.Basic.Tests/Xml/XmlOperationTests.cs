using Framework.Test;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Framework.Basic.Tests
{
    public class XmlOperationTests : TestBase
    {
        private XDocument _xDocument;
        public XmlOperationTests()
        {
            _xDocument = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xml", "Framework.AspNetCore.Test.xml"));
        }

        [Fact]
        public void Test1()
        {
            var targetTypeName = "Framework.AspNetCore.Test.Controllers.TestEnum";

            var attributes = _xDocument
                .Descendants("member")
                .Attributes("name")
                .Select(s => s.Value)
                .Where(w => w.Equals($"T:{targetTypeName}")).FirstOrDefault();

            attributes.ShouldBe($"T:{targetTypeName}");
        }

        protected override void LoadModules()
        {
            
        }
    }
}
