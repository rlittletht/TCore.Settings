using System;
using NUnit.Framework;

namespace TCore.Settings
{
	public class TestSynchronizeGetValues
	{
		class IntSettingValue
		{
#pragma warning disable 649
			[Setting("Setting1", 0, 0)] public int Setting1;
#pragma warning restore 649
		}

		[Test]
		public static void TestSyncIntSettingValue()
		{
			Settings settings = Settings.CreateSettings<IntSettingValue>("", "");
			IntSettingValue settingsClient = new IntSettingValue();

			settings.SetNValue("Setting1", 5);
			settings.SynchronizeGetValues<IntSettingValue>(settingsClient);
			Assert.AreEqual(5, settingsClient.Setting1);
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

			settings.SetNValue("Setting1", 5);
			settings.SynchronizeGetValues<IntSettingProperty>(settingsClient);
			Assert.AreEqual(5, settingsClient.Setting1);
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

			settings.SetFValue("Setting1", true);
			settings.SynchronizeGetValues<BoolSettingProperty>(settingsClient);
			Assert.AreEqual(true, settingsClient.Setting1);
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

			settings.SetSValue("Setting1", "NewValue");
			settings.SynchronizeGetValues<StringSettingProperty>(settingsClient);
			Assert.AreEqual("NewValue", settingsClient.Setting1);
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

			settings.SetDttmValue("Setting1", DateTime.Parse("1/1/2021"));
			settings.SynchronizeGetValues<DttmSettingProperty>(settingsClient);
			Assert.AreEqual(DateTime.Parse("1/1/2021"), settingsClient.Setting1);
		}

		class StrArraySettingProperty
		{
#pragma warning disable 169
			[Setting("Setting1", new string[] { }, new string[] { })] public string[] Setting1 { get; set; }
#pragma warning restore 169
		}

		[Test]
		public static void TestSync_StrArraySettingProperty()
		{
			Settings settings = Settings.CreateSettings<StrArraySettingProperty>("", "");
			StrArraySettingProperty settingsClient = new StrArraySettingProperty();

			settings.SetRgsValue("Setting1", new string[] { "First", "Second" });
			settings.SynchronizeGetValues<StrArraySettingProperty>(settingsClient);
			Assert.AreEqual(new string[] { "First", "Second" }, settingsClient.Setting1);
		}
	}
}