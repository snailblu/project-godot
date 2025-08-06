using NUnit.Framework;

namespace projectgodot.Tests
{
    [TestFixture]
    public class NewScriptTests
    {
        [Test]
        public void BasicMathTest()
        {
            int result = 2 + 2;
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void StringTest()
        {
            string testString = "Hello World";
            Assert.That(testString, Is.Not.Empty);
            Assert.That(testString, Does.Contain("World"));
        }
        
        [Test]
        public void NewScript_TypeExists()
        {
            var type = typeof(NewScript);
            Assert.That(type.Name, Is.EqualTo("NewScript"));
        }
    }
}