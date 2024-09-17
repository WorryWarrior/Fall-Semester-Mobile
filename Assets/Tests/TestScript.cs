using NUnit.Framework;

namespace Tests
{
    public class TestScript
    {
        [Test]
        public void TestScriptSimplePasses()
        {
            Assert.AreEqual(1,1);
        }
    }
}