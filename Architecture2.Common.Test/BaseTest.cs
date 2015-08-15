using System.Threading.Tasks;
using Architecture2.Common.Log4Net;
using log4net;
using NUnit.Framework;

namespace Architecture2.Common.Test
{
    public class BaseTest
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseTest));

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {

        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {

        }

        [SetUp]
        public virtual void SetUp()
        {
            Logger.Info(() => $"Setting up test {TestContext.CurrentContext.Test.FullName}");
        }

        [TearDown]
        public virtual void TearDown()
        {
            Logger.Info(() => $"Tearing down up test {TestContext.CurrentContext.Test.FullName}");
        }

        protected static async Task<T> Delayed<T>(int miliseconds, T result)
        {
            await Task.Delay(miliseconds).ConfigureAwait(false);
            return result;
        }

        protected static async Task<T> Delayed<T>(T result)
        {
            return await Delayed(10, result).ConfigureAwait(false);
        }


    }
}
