using Ecc.Implementation.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace CrocoLanding.Tests.Ecc
{
    [TestFixture]
    public class EccReplaceFuncTests
    {
        [Test]
        public void Test()
        {
            var text = "{{EccLink('crocosoft.ru')}}";

            var res = EccGetMasksService.ToReplacing(text);

            Assert.IsNotNull(res.Func);
            Assert.AreEqual("EccLink", res.Func.Name);
            Assert.AreEqual(new List<string>()
            {
                "'crocosoft.ru'"
            }, res.Func.Args);
        }

        [Test]
        public void Test2()
        {
            var text = "{{EccLink2('crocosoft.ru', '2', 3)}}";

            var res = EccGetMasksService.ToReplacing(text);

            Assert.IsNotNull(res.Func);
            Assert.AreEqual("EccLink2", res.Func.Name);
            Assert.AreEqual(new List<string>()
            {
                "'crocosoft.ru'",  "'2'", "3"
            }, res.Func.Args);
        }
    }
}