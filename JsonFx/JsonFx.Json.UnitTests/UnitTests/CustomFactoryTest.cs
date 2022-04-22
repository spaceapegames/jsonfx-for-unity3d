using NUnit.Framework;

namespace JsonFx.Json.Tests.UnitTests
{
    [TestFixture]
    public class CustomFactoryTest
    {
        public class MyFunnyType
        {
            [JsonIgnore]
            public int hilarity;

            public string setup;
            public string punchline;

            public MyFunnyType()
            : this(1)
            {
            }

            public MyFunnyType(int hilarity)
            {
                this.hilarity = hilarity;
            }
        }

        public class MyContainedType
        {
            public MyFunnyType funny;
        }
        
        [Test]
        public void GivenNonDefaultConstructor_WhenDeserializeJson_ThenConstructorUsed()
        {
            var jsonString = "{\"setup\":\"When is a door not a door?\",\"punchline\":\"When it's ajar\"}";
            var readerSettings = new JsonReaderSettings();
            readerSettings.CreateInstance = t => t == typeof(MyFunnyType) ? new MyFunnyType(100) : null;
            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyFunnyType>();
            Assert.AreEqual(100, joke.hilarity);
        }
        
        [Test]
        public void GivenCreateInstanceReturnsNull_WhenDeserializeJson_ThenDefaultConstructorUsed()
        {
            var jsonString = "{\"setup\":\"What's brown and sticky?\",\"punchline\":\"a stick\"}";
            var readerSettings = new JsonReaderSettings();
            readerSettings.CreateInstance = t => null;

            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyFunnyType>();
            Assert.AreEqual(1, joke.hilarity);
        }
        
        [Test]
        public void GivenNoCreateInstance_WhenDeserializeJson_ThenDefaultConstructorUsed()
        {
            var jsonString = "{\"setup\":\"What's brown and sticky?\",\"punchline\":\"a stick\"}";
            var readerSettings = new JsonReaderSettings();

            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyFunnyType>();
            Assert.AreEqual(1, joke.hilarity);
        }
        
        [Test]
        public void GivenNonDefaultConstructor_WhenDeserializeContainedJson_ThenConstructorUsed()
        {
            var jsonString = "{\"funny\":{\"setup\":\"When is a door not a door?\",\"punchline\":\"When it's ajar\"}}";
            var readerSettings = new JsonReaderSettings();
            readerSettings.CreateInstance = t => t == typeof(MyFunnyType) ? new MyFunnyType(100) : null;
            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyContainedType>();
            Assert.AreEqual(100, joke.funny.hilarity);
        }
        
        [Test]
        public void GivenCreateInstanceReturnsNull_WhenDeserializeContainedJson_ThenDefaultConstructorUsed()
        {
            var jsonString = "{\"funny\":{\"setup\":\"When is a door not a door?\",\"punchline\":\"When it's ajar\"}}";
            var readerSettings = new JsonReaderSettings();
            readerSettings.CreateInstance = t => null;
            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyContainedType>();
            Assert.AreEqual(1, joke.funny.hilarity);
        }
        
        [Test]
        public void GivenNoCreateInstance_WhenDeserializeContainedJson_ThenDefaultConstructorUsed()
        {
            var jsonString = "{\"funny\":{\"setup\":\"When is a door not a door?\",\"punchline\":\"When it's ajar\"}}";
            var readerSettings = new JsonReaderSettings();
            var joke = new JsonReader(jsonString, readerSettings).Deserialize<MyContainedType>();
            Assert.AreEqual(1, joke.funny.hilarity);
        }
    }
}