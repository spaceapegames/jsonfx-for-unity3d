using System;
using System.ComponentModel;
using System.Globalization;
using NUnit.Framework;

namespace JsonFx.Json.Tests.UnitTests
{
    [TestFixture]
    public class TestStructs
    {
        private JsonReaderSettings jsonReaderSettings;

        [TypeConverter(typeof(SimpleStructConverter))]
        public struct SimpleStruct
        {
            public int fieldOne;
            public int fieldTwo;
        }
        
        public class ClassWithArrayOfStructs
        {
            public SimpleStruct[] arrayOfStructs;
        }
        
        
        public class SimpleStructConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return new SimpleStruct();
            }
        }

        [SetUp]
        public void Setup()
        {
            jsonReaderSettings = new JsonReaderSettings();
            jsonReaderSettings.CreateInstance = type => Activator.CreateInstance(type);
            // jsonReaderSettings.AddTypeConverter(new SimpleStructConverter());
        }

        
        [TestCase("{}", -1),
         TestCase("{\"arrayOfStructs\":[]}", 0),
         TestCase("{\"arrayOfStructs\":[{}]}", 1),
         TestCase("{\"arrayOfStructs\":[{},{}]}", 2),
         TestCase("{\"arrayOfStructs\":[{},{},{}]}", 3)]
        public void GivenArrayOfStructs_WhenRead_ThenCorrectSizeRead(string json, int expectedSize)
        {
            var jsonReader = new JsonReader(json, jsonReaderSettings);
            var read = jsonReader.Deserialize<ClassWithArrayOfStructs>();
            Assert.AreEqual(expectedSize, (read.arrayOfStructs?.Length).GetValueOrDefault(-1));
        }
        
        [TestCase("{\"arrayOfStructs\":[{}]}",0,0,0),
         TestCase("{\"arrayOfStructs\":[null]}",0,0,0),
         TestCase("{\"arrayOfStructs\":[{\"fieldOne\":1,\"fieldTwo\":2}]}", 0,1,2),
         TestCase("{\"arrayOfStructs\":[{}, {\"fieldOne\":1,\"fieldTwo\":2}]}", 1, 1, 2),
         TestCase("{\"arrayOfStructs\":[{}, {\"fieldOne\":1,\"fieldTwo\":2}, {}]}", 2, 0, 0),
         TestCase("{\"arrayOfStructs\":[{}, {\"fieldOne\":1,\"fieldTwo\":2}, null]}", 2, 0, 0)]
        public void GivenArrayOfStructs_WhenRead_ThenFieldsCorrect(string json, int index, int expectedFieldOne, int expectedFieldTwo)
        {
            var jsonReader = new JsonReader(json, jsonReaderSettings);
            var read = jsonReader.Deserialize<ClassWithArrayOfStructs>();
            Assert.AreEqual(expectedFieldOne, read.arrayOfStructs[index].fieldOne);
            Assert.AreEqual(expectedFieldTwo, read.arrayOfStructs[index].fieldTwo);
        }
    }
}