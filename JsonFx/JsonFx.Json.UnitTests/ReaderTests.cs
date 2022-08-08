using NUnit.Framework;

namespace JsonFx.Json.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        public void TestDeserializationWithMixedTypeArrays()
        {
            // This test checks a case discovered when migrating to Unity 2021.
            // This threw an InvalidCastException in Unity 2021 but not when testing in 2019.
            // It also did not throw when running the tests directly in this sln.
            var deserialized = JsonReader.Deserialize("[[\"id\",\"name\"],[1,\"myname\"]]");

            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOf<object[][]>(deserialized);
        }
    }
}
