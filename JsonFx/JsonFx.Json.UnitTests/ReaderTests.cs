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

        [Test]
        public void TestDeserializationWithHomogeneousArrays()
        {
            string strJson = "[[\"id\",\"name\"],[\"1\",\"myname\"]]";
            TestDeserialization<string[][]>(strJson);

            string intJson = "[[1,2],[3,4]]";
            TestDeserialization<int[][]>(intJson);

            string doubleJson = "[[1.2,2.3],[3.5,4.3]]";
            TestDeserialization<double[][]>(doubleJson);
        }

        private void TestDeserialization<T>(string json)
        {
            var deserialized = JsonReader.Deserialize(json);
            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOf<T>(deserialized);
        }
    }
}
