using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TCore.Settings
{
	public class SettingAttribute : Attribute
	{
		public Settings.Type ?SettingsType { get; set; }
		public string Key { get; set; }
		public object ModelSourceOrInitialValue { get; set; }
		public object DefaultRegistryValue { get; set; }

		public SettingAttribute(Settings.Type type, string key, object modelSourceOrInitialValue, object defaultRegistryValue)
		{
			SettingsType = type;
			DefaultRegistryValue = defaultRegistryValue;
			ModelSourceOrInitialValue = modelSourceOrInitialValue;
			Key = key;
		}

		public SettingAttribute(Settings.Type type, object modelSourceOrInitialValue, object defaultRegistryValue)
		{
			SettingsType = type;
			DefaultRegistryValue = defaultRegistryValue;
			ModelSourceOrInitialValue = modelSourceOrInitialValue;
			Key = null;
		}

		public SettingAttribute(string key, object modelSourceOrInitialValue, object defaultRegistryValue)
		{
			SettingsType = null;
			DefaultRegistryValue = defaultRegistryValue;
			ModelSourceOrInitialValue = modelSourceOrInitialValue;
			Key = key;
		}

		public SettingAttribute(object modelSourceOrInitialValue, object defaultRegistryValue)
		{
			SettingsType = null;
			DefaultRegistryValue = defaultRegistryValue;
			ModelSourceOrInitialValue = modelSourceOrInitialValue;
			Key = null;
		}
	}

	public class SettingAttributes
	{
		class SingleIntValueSetting
		{
			[Setting(Settings.Type.Int, 0, 0)]
#pragma warning disable 649
			public int IntTest;
#pragma warning restore 649
		}

		[Test]
		public static void TestBasicIntValueTypeSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntValueSetting>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);
		}

		class SingleIntValueSettingInferredType
		{
			[Setting(0, 0)]
#pragma warning disable 649
			public int IntTest;
#pragma warning restore 649
		}

		[Test]
		public static void TestBasicIntValueTypeSettingInferred()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntValueSettingInferredType>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);
		}

		class SingleIntSetting
		{
			[Setting(Settings.Type.Int, 0, 0)]
			public int IntTest { get; set; }
		}

		[Test]
		public static void TestBasicIntTypeSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntSetting>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);
		}

		class SingleIntSettingInferred
		{
			[Setting(0, 0)]
			public int IntTest { get; set; }
		}

		[Test]
		public static void TestBasicIntTypeSettingInferred()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntSettingInferred>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);
		}

		class SingleDttmSetting
		{
			[Setting(Settings.Type.Dttm, "", "")]
			public DateTime DttmTest { get; set; }
		}

		[Test]
		public static void TestBasicDttmTypeSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleDttmSetting>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Dttm, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.String);
			Assert.AreEqual("", rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.String);
			Assert.AreEqual("", rgehe[0].oref);
			Assert.AreEqual("DttmTest", rgehe[0].sRePath);
		}

		class SingleDttmSettingInferred
		{
			[Setting("", "")]
			public DateTime DttmTest { get; set; }
		}

		[Test]
		public static void TestBasicDttmTypeSettingInferred()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleDttmSettingInferred>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Dttm, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.String);
			Assert.AreEqual("", rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.String);
			Assert.AreEqual("", rgehe[0].oref);
			Assert.AreEqual("DttmTest", rgehe[0].sRePath);
		}

		class SingleStrSetting
		{
			[Setting(Settings.Type.Str, "", "")]
			public string[] StrTest { get; set; }
		}

		[Test]
		public static void TestBasicStrTypeSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleStrSetting>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Str, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.String);
			Assert.AreEqual("", rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.String);
			Assert.AreEqual("", rgehe[0].oref);
			Assert.AreEqual("StrTest", rgehe[0].sRePath);
		}

		class SingleStrSettingInferred
		{
			[Setting("", "")]
			public string StrTest { get; set; }
		}

		[Test]
		public static void TestBasicStrTypeSettingInferred()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleStrSettingInferred>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Str, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.String);
			Assert.AreEqual("", rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.String);
			Assert.AreEqual("", rgehe[0].oref);
			Assert.AreEqual("StrTest", rgehe[0].sRePath);
		}

		class SingleStrSettingMisMatchButDeclared
		{
			[Setting(Settings.Type.StrArray, new string[] { }, new string[] { })]
			public List<string> StrTest { get; set; }
		}

		[Test]
		public static void TestBasicStrSettingMisMatchButDeclared()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleStrSettingMisMatchButDeclared>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.StrArray, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is string[]);
			Assert.AreEqual("", rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is string[]);
			Assert.AreEqual("", rgehe[0].oref);
			Assert.AreEqual("StrTest", rgehe[0].sRePath);
		}

		class SingleStrArraySetting
		{
			[Setting(Settings.Type.StrArray, new string[] { }, new string[] { })]
			public string[] StrArrayTest { get; set; }
		}

		[Test]
		public static void TestBasicStrArrayTypeSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleStrArraySetting>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.StrArray, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is string[]);
			Assert.AreEqual(0, ((string[])rgehe[0].oDefault).Length);
			Assert.IsTrue(rgehe[0].oref is string[]);
			Assert.AreEqual(0, ((string[])rgehe[0].oref).Length);
			Assert.AreEqual("StrArrayTest", rgehe[0].sRePath);
		}

		class SingleStrArraySettingInferred
		{
			[Setting(new string[] { }, new string[] { })]
			public string[] StrArrayTest { get; set; }
		}

		[Test]
		public static void TestBasicStrArrayTypeSettingInferred()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleStrArraySettingInferred>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.StrArray, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is string[]);
			Assert.AreEqual(0, ((string[])rgehe[0].oDefault).Length);
			Assert.IsTrue(rgehe[0].oref is string[]);
			Assert.AreEqual(0, ((string[])rgehe[0].oref).Length);
			Assert.AreEqual("StrArrayTest", rgehe[0].sRePath);
		}

		class SingleIntSettingWithPath
		{
			[Setting(Settings.Type.Int, "LastIntTest", 0, 0)]
			public int IntTest { get; set; }
		}

		[Test]
		public static void TestBasicIntTypeWithPathSetting()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntSettingWithPath>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("LastIntTest", rgehe[0].sRePath);
		}

		class SingleIntSettingIgnoreOtherMembers
		{
			[Setting(Settings.Type.Int, 0, 0)]
			public int IntTest { get; set; }

			public bool ToBeIgnored { get; set; }
		}

		[Test]
		public static void TestBasicIntSetting_WithIgnoredMembers()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<SingleIntSettingIgnoreOtherMembers>();

			Assert.AreEqual(1, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);
		}

		class IntBoolSetting
		{
			[Setting(Settings.Type.Int, 0, 0)]
			public int IntTest { get; set; }

			[Setting(Settings.Type.Bool, false, 0)]
			public bool BoolTest{ get; set; }
		}

		[Test]
		public static void TestBasicIntBoolSettings()
		{
			Settings.SettingsElt[] rgehe = Settings.SettingsElt.CreateSettings<IntBoolSetting>();

			Assert.AreEqual(2, rgehe.Length);
			Assert.AreEqual(Settings.Type.Int, rgehe[0].type);
			Assert.IsTrue(rgehe[0].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[0].oDefault);
			Assert.IsTrue(rgehe[0].oref is System.Int32);
			Assert.AreEqual(0, rgehe[0].oref);
			Assert.AreEqual("IntTest", rgehe[0].sRePath);

			Assert.AreEqual(Settings.Type.Bool, rgehe[1].type);
			Assert.IsTrue(rgehe[1].oDefault is System.Int32);
			Assert.AreEqual(0, rgehe[1].oDefault);
			Assert.IsTrue(rgehe[1].oref is System.Boolean);
			Assert.AreEqual(false, rgehe[1].oref);
			Assert.AreEqual("BoolTest", rgehe[1].sRePath);
		}
	}
}