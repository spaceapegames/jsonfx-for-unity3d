﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonFx.Json.Tests;

namespace JsonFx.Json.Tests
{
	public class FieldsClass {
		public int anInt;
		public string aString;
		public bool aBool;
		public float aFloat;
	}
	
	public class PublishedPropertiesClass {
		private int _anInt;
		public int anInt { get { return _anInt; } set { _anInt = value; } } 
		private string _aString;
		public string aString { get { return _aString; } set { _aString = value; } } 
		private bool _aBool;
		public bool aBool { get { return _aBool; } set { _aBool = value; } } 
		private float _aFloat;
		public float aFloat { get { return _aFloat; } set { _aFloat = value; } } 
	}
	
	public class AutoPropertiesClass {
		public int anInt { get; set; }
		public string aString { get; set; }
		public bool aBool { get; set; }
		public float aFloat { get; set; }
	}
	
	public class ArraysClass {
		public int[] ints;
		public string[] strings;
		public bool[] bools;
		public float[] floats;
		public FieldsClass[] fields;
	}
	
	public class ListsClass {
		public List<int> ints;
		public List<string> strings;
		public List<bool> bools;
		public List<float> floats;
		public List<FieldsClass> fields;
	}
	
	public class ReadonlyListsClass {
		public readonly List<int> ints = new List<int>();
		public readonly List<string> strings = new List<string>();
		public readonly List<bool> bools = new List<bool>();
		public readonly List<float> floats = new List<float>();
		public readonly List<FieldsClass> fields = new List<FieldsClass>();
	}
	
	public class PublicSetPropertiesClass {
		public List<int> ints { get; private set; }
		public List<string> strings { get; private set; }
		public List<bool> bools { get; private set; }
		public List<float> floats { get; private set; }
		public List<FieldsClass> fields { get; private set; }
		
		public PublicSetPropertiesClass() {
			ints = new List<int>();
			strings = new List<string>();
			bools = new List<bool>();
			floats = new List<float>();
			fields = new List<FieldsClass>();
		}
	}
	
	public class GetOnlyListPropertiesClass {
		private List<int> _ints = new List<int>();
		public List<int> ints { get { return _ints; } }
		private List<string> _strings = new List<string>();
		public List<string> strings { get { return _strings; } }
		private List<bool> _bools = new List<bool>();
		public List<bool> bools { get { return _bools; } }
		private List<float> _floats = new List<float>();
		public List<float> floats { get { return _floats; } }
		private List<FieldsClass> _fields = new List<FieldsClass>();
		public List<FieldsClass> fields { get { return _fields; } }
	}
	
	public class BigNestedClass {
		public FieldsClass fields;
		public PublishedPropertiesClass publishedProperties;
		public AutoPropertiesClass autoProperties;
		public ArraysClass[] array;
		public List<ListsClass> list;
		public readonly List<ReadonlyListsClass> readonlyList = new List<ReadonlyListsClass>();
		public List<PublicSetPropertiesClass> propertyList { get; private set; }
		
		public BigNestedClass() {
			propertyList = new List<PublicSetPropertiesClass>();
		}
	}
	
	[TestFixture()]
	public class ClassTests
	{
		[Test()]
		public void TestFields()
		{
			DoTests<FieldsClass>();
		}
		
		[Test()]
		public void TestPublishedPropertiesClass()
		{
			DoTests<PublishedPropertiesClass>();
		}
		
		[Test()]
		public void TestAutoPropertiesClass()
		{
			DoTests<AutoPropertiesClass>();
		}
		
		[Test()]
		public void TestArraysClass()
		{
			DoTests<ArraysClass>();
		}
		
		[Test()]
		public void TestListsClass()
		{
			DoTests<ListsClass>();
		}
		
		[Test()]
		public void TestReadonlyListsClass()
		{
			DoTests<ReadonlyListsClass>();
		}
		
		[Test()]
		public void TestPublicSetPropertiesClass()
		{
			DoTests<PublicSetPropertiesClass>();
		}
		
		[Test()]
		public void TestGetOnlyListPropertiesClass()
		{
			DoTests<GetOnlyListPropertiesClass>();
		}
		
		[Test()]
		public void TestBigNestedClass()
		{
			DoTests<BigNestedClass>();
		}
		
		private void DoTests<T>() {
			var original = FuzzUtil.FuzzGen<T>();
			BasicTest(original);
			BasicTestWithSettings(original, new JsonWriterSettings() { PrettyPrint = true });
			BasicTestWithSettings(original, new JsonWriterSettings() { PrettyMergeable = true });
			BasicTestWithSettings(original, new JsonWriterSettings() { SortMembers = true });
		}

		private static void BasicTest<T>(T original)
		{
			var serialized = JsonWriter.Serialize(original);
//			Console.WriteLine("Test serialize: " + serialized);
			var deserialized = JsonReader.Deserialize<T>(serialized);
//			Assert.AreEqual(original, deserialized);
			var reserialized = JsonWriter.Serialize(deserialized);
			Assert.AreEqual(serialized, reserialized);
		}

		private static void BasicTestWithSettings<T>(T original, JsonWriterSettings settings)
		{
			var stringBuilder = new StringBuilder();
			var writer = new JsonWriter(stringBuilder, settings);
			writer.Write(original);
			var serialized = stringBuilder.ToString();
//			Console.WriteLine("Test serialize: " + serialized);
			var deserialized = JsonReader.Deserialize<T>(serialized);
//			Assert.AreEqual(original, deserialized);
			stringBuilder.Length = 0;
			stringBuilder.Capacity = 0;
			writer.Write(deserialized);
			var reserialized = stringBuilder.ToString();
			Assert.AreEqual(serialized, reserialized);
		}
	}
}

