﻿// ============================================================================
// S E T T I N G S
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TCore.Settings
{
    public partial class Settings //STE
    {
        #region Data model

        private SettingsElt[] m_rgstee;
        private string m_sRoot;
        private string m_sTag;

        public enum Type
        {
            Bool = 1,
            Str = 2,
            Int = 3,
            Dttm = 4,
            StrArray = 5
        };

        public struct SettingsElt // STEE
        {
            public string sRePath;
            public Type type;
            public object oref;
            public object oDefault;

            public SettingsElt(string sRegistryPath, Type typ, object oRefToSync, object oDefault)
            {
                sRePath = sRegistryPath;
                type = typ;
                oref = oRefToSync;
                this.oDefault = oDefault;
            }

            public SettingsElt Clone()
            {
                return new SettingsElt(sRePath, type, oref, oDefault);
            }
        };

        #endregion

        #region Public I/O Methods

        /* S A V E */
        /*----------------------------------------------------------------------------
        	%%Function: Save
        	%%Qualified: TCore.Settings.Settings.Save
        	%%Contact: rlittle
        	
        ----------------------------------------------------------------------------*/
        public void Save()
        {
            RegistryKey rk = RkEnsure(m_sRoot);

            foreach (SettingsElt stee in m_rgstee)
                {
                object oVal = OFromOref(stee.oref);

                if (oVal == null)
                    continue;

                int nT;

                switch (stee.type)
                    {
                    case Type.StrArray:
                        string[] rgs = (string[]) oVal;
                        rk.SetValue(stee.sRePath, rgs, RegistryValueKind.MultiString);
                        break;
                    case Type.Dttm:
                        DateTime dttm;
                        if (oVal is string)
                            {
                            if (String.IsNullOrEmpty((string) oVal))
                                {
                                dttm = DateTime.Now;
                                dttm = new DateTime(dttm.Year, dttm.Month, dttm.Day, dttm.Hour, 0, 0);
                                }
                            else
                                dttm = DateTime.Parse((string) oVal);
                            }
                        else
                            dttm = (DateTime) oVal;
                        rk.SetValue(stee.sRePath, dttm.ToString());
                        break;
                    case Type.Str:
                        string sT = (string) oVal;
                        rk.SetValue(stee.sRePath, sT);
                        break;
                    case Type.Bool:
                        if (oVal is bool)
                            nT = (bool) oVal ? 1 : 0;
                        else
                            {
                            nT = ((bool) oVal) ? 1 : 0;
                            }
                        rk.SetValue(stee.sRePath, nT, RegistryValueKind.DWord);
                        break;
                    case Type.Int:
                        if (oVal is int)
                            nT = (int) oVal;
                        else
                            {
                            nT = Int32.Parse((string) oVal);
                            }
                        rk.SetValue(stee.sRePath, nT, RegistryValueKind.DWord);
                        break;
                    }
                }
        }

        /* L O A D */
        /*----------------------------------------------------------------------------
        	%%Function: Load
        	%%Qualified: TCore.Settings.Settings.Load
        	%%Contact: rlittle
        	
        ----------------------------------------------------------------------------*/
        public void Load()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(m_sRoot);
            string sVal = "";
            string[] rgs = null;
            DateTime dttmVal = DateTime.Today;
            int nVal = 0;
            bool fVal = false;
            int nT;
            string sT;

            if (rk == null)
                return;

            for (int i = 0; i < m_rgstee.Length; i++)
                {
                SettingsElt rehe = m_rgstee[i];

                switch (rehe.type)
                    {
                    case Type.StrArray:
                        rgs = (string[]) rk.GetValue(rehe.sRePath, rehe.oDefault);
                        break;
                    case Type.Dttm:
                        sT = (string) rk.GetValue(rehe.sRePath, rehe.oDefault);
                        sVal = sT;
                        DateTime.TryParse(sT, out dttmVal);
                        break;
                    case Type.Str:
                        sT = (string) rk.GetValue(rehe.sRePath, rehe.oDefault);
                        sVal = sT;

                        if (!Int32.TryParse(sT, out nVal))
                            nVal = 0;
                        break;
                    case Type.Bool:
                        nT = (int) rk.GetValue(rehe.sRePath, rehe.oDefault);
                        fVal = (nT != 0 ? true : false);
                        break;
                    case Type.Int:
                        nT = (int) rk.GetValue(rehe.sRePath, rehe.oDefault);
                        sVal = nT.ToString();
                        nVal = nT;
                        break;
                    }

                if (rehe.oref is System.Windows.Forms.TextBox)
                    {
                    ((System.Windows.Forms.TextBox) rehe.oref).Text = sVal;
                    }
                else if (rehe.oref is System.Windows.Forms.ListBox)
                    {
                    ((System.Windows.Forms.ListBox) rehe.oref).Text = sVal;
                    }
                else if (rehe.oref is System.Windows.Forms.ComboBox)
                    {
                    ((System.Windows.Forms.ComboBox) rehe.oref).Text = sVal;
                    }
                else if (rehe.oref is System.Windows.Forms.CheckBox)
                    {
                    ((System.Windows.Forms.CheckBox) rehe.oref).Checked = fVal;
                    }
                else if (rehe.oref is System.Windows.Forms.DateTimePicker)
                    {
                    ((System.Windows.Forms.DateTimePicker) rehe.oref).Value = dttmVal;
                    }
                else if (rehe.oref is string[])
                    {
                    m_rgstee[i].oref = rgs;
                    }
                else if (rehe.oref is string)
                    {
                    m_rgstee[i].oref = (string) sVal;
                    }
                else if (rehe.oref is Int32)
                    {
                    m_rgstee[i].oref = (Int32) nVal;
                    }
                else if (rehe.oref is DateTime)
                    {
                    m_rgstee[i].oref = dttmVal;
                    }
                else if (rehe.oref is bool)
                    {
                    m_rgstee[i].oref = fVal;
                    }
                else if (rehe.oref is Int16)
                    {
                    m_rgstee[i].oref = (Int16) nVal;
                    }

                else
                    {
                    throw (new Exception("Unkonwn control type in Settings.Save"));
                    }
                }
        }

        #endregion

        #region Public Value accessors

        public string SValue(string sKey)
        {
            return (string) OFindValue(sKey);
        }

        public string[] RgsValue(string sKey)
        {
            return (string[]) OFindValue(sKey);
        }
        public bool FValue(string sKey)
        {
            object o = OFindValue(sKey);

            string sValue;

            if (o is bool)
                sValue = ((bool) o).ToString();
            else if (o is int)
                sValue = ((int) o).ToString();
            else
                sValue = (string) o;

            if (String.Compare(sValue, "true", true) == 0
                || String.Compare(sValue, "1") == 0)
                return true;
            else
                return false;
        }

        public void SetRgsValue(string sKey, string[] rgsValue)
        {
            int i = IFindKey(sKey);
            if (i != -1)
                m_rgstee[i].oref = rgsValue;
            else
                throw new Exception("could not find given key");
        }

        public void SetSValue(string sKey, string sValue)
        {
            int i = IFindKey(sKey);
            if (i != -1)
                m_rgstee[i].oref = sValue;
            else
                throw new Exception("could not find given key");
        }

        public void SetNValue(string sKey, int nValue)
        {
            int i = IFindKey(sKey);
            if (i != -1)
                m_rgstee[i].oref = nValue;
            else
                throw new Exception("could not find given key");
        }

        public void SetFValue(string sKey, bool fValue)
        {
            int i = IFindKey(sKey);
            if (i != -1)
                m_rgstee[i].oref = fValue;
            else
                throw new Exception("could not find given key");
        }

        public Int16 WValue(string sKey)
        {
            return (Int16) OFindValue(sKey);
        }

        public Int32 NValue(string sKey)
        {
            return (Int32) OFindValue(sKey);
        }

        public DateTime DttmValue(string sKey)
        {
            return (DateTime) OFindValue(sKey);
        }

        public bool FMatchesTag(string sTag)
        {
            return String.Compare(sTag, m_sTag) == 0;
        }

        public Settings(SettingsElt[] rgstee, string sReRoot, string sTag)
        {
            m_rgstee = new SettingsElt[rgstee.Length];

            for (int i = 0; i < rgstee.Length; i++)
                m_rgstee[i] = rgstee[i].Clone();

            m_sRoot = sReRoot;
            m_sTag = sTag;
        }

        public static RegistryKey RkEnsure(string sRoot)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(sRoot, true);

            if (rk == null)
                {
                rk = Registry.CurrentUser.CreateSubKey(sRoot);
                if (rk == null)
                    return null;
                }
            return rk;
        }

        public string Tag => m_sTag;

        #endregion

        #region Internals

        /* O  F I N D  V A L U E */
        /*----------------------------------------------------------------------------
        	%%Function: OFindValue
        	%%Qualified: TCore.Settings.Settings.OFindValue
        	%%Contact: rlittle
        	
        ----------------------------------------------------------------------------*/
        object OFindValue(string sKey)
        {
            foreach (SettingsElt stee in m_rgstee)
                {
                if (string.Compare(stee.sRePath, sKey) == 0)
                    return stee.oref;
                }
            return null;
        }

        int IFindKey(string sKey)
        {
            for (int i = 0; i < m_rgstee.Length; i++)
                {
                if (string.Compare(m_rgstee[i].sRePath, sKey) == 0)
                    {
                    return i;
                    }
                }
            return -1;
        }

        object OFromOref(object oref)
        {
            object oVal;

            if (oref is System.Windows.Forms.TextBox)
                {
                oVal = ((System.Windows.Forms.TextBox) oref).Text;
                }
            else if (oref is System.Windows.Forms.ComboBox)
                {
                oVal = ((System.Windows.Forms.ComboBox) oref).Text;
                }
            else if (oref is System.Windows.Forms.ListBox)
                {
                oVal = ((System.Windows.Forms.ListBox) oref).Text;
                }
            else if (oref is System.Windows.Forms.CheckBox)
                {
                oVal = ((System.Windows.Forms.CheckBox) oref).Checked;
                }
            else if (oref is System.Windows.Forms.DateTimePicker)
                {
                oVal = ((System.Windows.Forms.DateTimePicker) oref).Value;
                }
            else if (oref is string || oref is Int32 || oref is DateTime || oref is bool || oref is Int16 || oref is string[])
                {
                oVal = oref;
                }
            else
                {
                oVal = null;
                }
            return oVal;
        }

        #endregion

        /* R G S  G E T  S U B K E Y S */
        /*----------------------------------------------------------------------------
        	%%Function: RgsGetSubkeys
        	%%Qualified: TCore.Settings.Settings.RgsGetSubkeys
        	%%Contact: rlittle
        	
            return a simple list of the subkey names underneath this registry root
        ----------------------------------------------------------------------------*/
        public static string[] RgsGetSubkeys(string sRegRoot)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(sRegRoot);

            if (rk != null)
                {
                string[] rgs = rk.GetSubKeyNames();
                rk.Close();
                return rgs;
                }
            return null;
        }
    }
}
