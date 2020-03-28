using Croco.Core.Abstractions;
using Ecc.Implementation.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace CrocoLanding.Tests.Ecc
{
    [TestFixture]
    public class ProcessTextViaFunctionsTests
    {
        public class TestFunctionInvoker : IEccTextFunctionInvoker
        {
            public string ProccessText(string interactionId, EccReplacing replacing, ICrocoAmbientContext ambientContext)
            {
                return "Test";
            }
        }

        [Test]
        public void Test3ViaFuncs()
        {
            var text = "{{EccLink('sometext')}} abc {{EccLink('someurl')}}";

            var dict = new Dictionary<string, IEccTextFunctionInvoker>
            {
                ["EccLink"] = new TestFunctionInvoker()
            };

            var result = EccGetMasksService.ProcessTextViaFunctions(text, "interactionId", Substitute.For<ICrocoAmbientContext>(), dict);

            Assert.AreEqual("Test abc Test", result);
        }
    }
}