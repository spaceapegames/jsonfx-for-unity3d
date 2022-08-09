using NUnit.Framework;

namespace JsonFx.Json.Tests
{
    [TestFixture]
    public class ReaderTests
    {
        [Test]
        [TestCase("[[\"id\",\"name\"],[1,\"myname\"]]")]
        [TestCase("[[\"id\",\"name\"],[null]]")]
        [TestCase("[[\"id\",\"name\"],[1, null]]")]
        [TestCase("[[null,\"name\"],[{}, {}]]")]
        [TestCase("[[null,\"name\"],[1, true]]")]
        [TestCase("[[false,\"name\"],[1, null]]")]
        [TestCase("[[{},{}],null]")]
        public void TestDeserializationWithMixedTypeArrays(string json)
        {
            // This test checks a case discovered when migrating to Unity 2021.
            // This threw an InvalidCastException in Unity 2021 but not when testing in 2019.
            // It also did not throw when running the tests directly in this sln.
            var deserialized = JsonReader.Deserialize(json);
            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOf<object[][]>(deserialized);
        }

        [Test]
        public void TestDeserializationWithStringArrays()
        {
            string json = "[[\"id\",\"name\"],[\"1\",\"myname\"]]";
            TestDeserialization<string[][]>(json);
        }

        [Test]
        public void TestDeserializationWithIntArrays()
        {
            string json = "[[1,2],[3,4]]";
            TestDeserialization<int[][]>(json);
        }

        [Test]
        public void TestDeserializationWithDoubleArrays()
        {
            string json = "[[1.2,2.3],[3.5,4.3]]";
            TestDeserialization<double[][]>(json);
        }

        [Test]
        public void TestDeserializationWithBoolArrays()
        {
            string json = "[[true, false],[true, true]]";
            TestDeserialization<bool[][]>(json);
        }

        private void TestDeserialization<T>(string json)
        {
            var deserialized = JsonReader.Deserialize(json);
            Assert.IsNotNull(deserialized);
            Assert.IsInstanceOf<T>(deserialized);
        }
    }
}
