using System;
using NUnit.Framework;

namespace TCore.Settings
{
	public class TestSynchronizeSetValues
	{
		class IntSettingValue
		{
#pragma warning disable 169
			[Setting("Setting1", 0, 0)] public int Setting1;
#pragma warning restore 169
		}

		[Test]
		public static void TestSyncIntSettingValue()
		{
			Settings settings = Settings.CreateSettings<IntSettingValue>("", "");
			IntSettingValue settingsClient = new IntSettingValue();

			settingsClient.Setting1 = 5;
			Assert.AreEqual(0, settings.NValue("Setting1"));
			settings.SynchronizeSetValues<IntSettingValue>(settingsClient);
			Assert.AreEqual(5, settings.NValue("Setting1"));
		}

		class IntSettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", 0, 0)] public int Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_IntSettingProperty()
		{
			Settings settings = Settings.CreateSettings<IntSettingProperty>("", "");
			IntSettingProperty settingsClient = new IntSettingProperty();

			settingsClient.Setting1 = 5;
			Assert.AreEqual(0, settings.NValue("Setting1"));
			settings.SynchronizeSetValues<IntSettingProperty>(settingsClient);
			Assert.AreEqual(5, settings.NValue("Setting1"));
		}

		class BoolSettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", false, 0)] public bool Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_BoolSettingProperty()
		{
			Settings settings = Settings.CreateSettings<BoolSettingProperty>("", "");
			BoolSettingProperty settingsClient = new BoolSettingProperty();

			settingsClient.Setting1 = true;
			Assert.AreEqual(false, settings.FValue("Setting1"));
			settings.SynchronizeSetValues<BoolSettingProperty>(settingsClient);
			Assert.AreEqual(true, settings.FValue("Setting1"));
		}

		class StringSettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", "default", "")] public string Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_StringSettingProperty()
		{
			Settings settings = Settings.CreateSettings<StringSettingProperty>("", "");
			StringSettingProperty settingsClient = new StringSettingProperty();

			settingsClient.Setting1 = "NewValue";
			Assert.AreEqual("default", settings.SValue("Setting1"));
			settings.SynchronizeSetValues<StringSettingProperty>(settingsClient);
			Assert.AreEqual("NewValue", settings.SValue("Setting1"));
		}

		class DttmSettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", "", "")] public DateTime Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_DttmSettingProperty()
		{
			Settings settings = Settings.CreateSettings<DttmSettingProperty>("", "");
			DttmSettingProperty settingsClient = new DttmSettingProperty();

			settingsClient.Setting1 = DateTime.Parse("1/1/2021");
			Assert.AreEqual(DateTime.MinValue, settings.DttmValue("Setting1"));
			settings.SynchronizeSetValues<DttmSettingProperty>(settingsClient);
			Assert.AreEqual(DateTime.Parse("1/1/2021"), settings.DttmValue("Setting1"));
		}

		class StrArraySettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", new string[] {}, new string[] { })] public string[] Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_StrArraySettingProperty()
		{
			Settings settings = Settings.CreateSettings<StrArraySettingProperty>("", "");
			StrArraySettingProperty settingsClient = new StrArraySettingProperty();

			settingsClient.Setting1 = new string[] {"First", "Second"};

			Assert.AreEqual(new string[] {}, settings.RgsValue("Setting1"));
			settings.SynchronizeSetValues<StrArraySettingProperty>(settingsClient);
			Assert.AreEqual(new string[] {"First", "Second"}, settings.RgsValue("Setting1"));
		}
	}
}