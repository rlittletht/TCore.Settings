﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using NUnit.Framework;

namespace TCore.Settings
{
    public partial class Settings //STE
    {
        public Settings()
        {
        }

        [Test]
        public void TestRkEnsureNew()
        {
            RegistryKey rk;
            
            rk = Registry.CurrentUser.OpenSubKey("Software", true);
            try
                {
                rk.DeleteSubKeyTree("__UnitTestTest_TcoreSettings__");
                }
            catch
                {
                }
            rk.Close();
            rk = RkEnsure("Software\\__UnitTestTest_TcoreSettings__\\test");

            Assert.AreNotEqual(null, rk);
            Assert.AreEqual("HKEY_CURRENT_USER\\Software\\__UnitTestTest_TcoreSettings__\\test", rk.Name);

            rk.Close();

            rk = Registry.CurrentUser.OpenSubKey("Software", true);
            rk.DeleteSubKeyTree("__UnitTestTest_TcoreSettings__");
        }

        [Test]
        public void TestRkEnsureExisting()
        {
            RegistryKey rk;
            
            rk = Registry.CurrentUser.OpenSubKey("Software", true);
            try
                {
                rk.DeleteSubKeyTree("__UnitTestTest_TcoreSettings__");
                }
            catch
                {
                }
            rk.CreateSubKey("__UnitTestTest_TcoreSettings__\\test");
            rk.Close();
            rk.Close();
            rk = RkEnsure("Software\\__UnitTestTest_TcoreSettings__\\test");

            Assert.AreNotEqual(null, rk);
            Assert.AreEqual("HKEY_CURRENT_USER\\Software\\__UnitTestTest_TcoreSettings__\\test", rk.Name);

            rk.Close();

            rk = Registry.CurrentUser.OpenSubKey("Software", true);
            rk.DeleteSubKeyTree("__UnitTestTest_TcoreSettings__");
        }

        [Test]
        public void TestOFromOref_TextBox()
        {
            TextBox oRef = new TextBox();
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_ComboBox()
        {
            ComboBox oRef = new ComboBox();
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_ListBox()
        {
            ListBox oRef = new ListBox();
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_CheckBox()
        {
            CheckBox oRef = new CheckBox();
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_DateTimePicker()
        {
            DateTimePicker oRef = new DateTimePicker();
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_string()
        {
            string oRef = "foo";
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_Int32()
        {
            Int32 oRef = 21;
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_DateTime()
        {
            DateTime oRef = DateTime.Parse("1/1/2000");
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_bool()
        {
            bool oRef = true;
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_Int16()
        {
            Int16 oRef = 12;
            object o = OFromOref(oRef);

            Assert.AreNotEqual(null, o);
        }

        [Test]
        public void TestOFromOref_Unknown()
        {
            Int64 oRef = 12;
            object o = OFromOref(oRef);

            Assert.AreEqual(null, o);
        }

        [Test]
        public void TestValueFromKey()
        {
            Settings ste = new Settings(new SettingsElt[]
                {
                new SettingsElt("TestString1", Settings.Type.Str, "1", ""),
                new SettingsElt("TestString2", Settings.Type.Str, "2", ""),
                new SettingsElt("TestInt161", Settings.Type.Int, (Int16) 1, ""),
                new SettingsElt("TestInt321", Settings.Type.Int, (Int32) 2, ""),
                new SettingsElt("TestDttm1", Settings.Type.Dttm, DateTime.Parse("1/1/1901"), ""),
                },
                                        "__UnitTestTest_TcoreSettings__", "tag");

            Assert.AreEqual("1", ste.SValue("TestString1"));
            Assert.AreEqual(1, ste.WValue("TestInt161"));
            Assert.AreEqual(2, ste.NValue("TestInt321"));
            Assert.AreEqual(DateTime.Parse("1/1/1901"), ste.DttmValue("TestDttm1"));
        }

    }
}
