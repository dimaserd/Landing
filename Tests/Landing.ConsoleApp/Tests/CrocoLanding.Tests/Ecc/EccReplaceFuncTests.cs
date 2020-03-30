using Ecc.Logic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Croco.Testing.Impelementations;
using System.Linq;
using Ecc.Model.Entities.LinkCatch;

namespace CrocoLanding.Tests.Ecc
{
    [TestFixture]
    public class EccReplaceFuncTests
    {
        [Test]
        public void TestEccLinkFunctionInvoker()
        {
            var invoker = new EccLinkFunctionInvoker("https://domain.com?RedirId={0}");

            var ambContext = new TestCrocoAmbientContext();

            var mailId = Guid.NewGuid().ToString();

            var text = invoker.ProccessText(mailId, new EccReplacing
            {
                Func = new EccTextFunc
                {
                    Args = new List<string>
                    {
                        "'crocosoft.ru'"
                    },
                    Name = "SomeName"
                },
                TextToReplace = "SomeText"
            }, ambContext);

            Assert.IsTrue(text.StartsWith("https://domain.com?RedirId="));

            //Сохраняем то что добавил процессор текста
            ambContext.RepositoryFactory.SaveChangesAsync().Wait();

            var linkCatch = ambContext.TestRepositoryFactory.GetDataList<EmailLinkCatch>().First();

            Assert.AreEqual(mailId, linkCatch.MailMessageId);
            Assert.AreEqual("crocosoft.ru", linkCatch.Url);
        }

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