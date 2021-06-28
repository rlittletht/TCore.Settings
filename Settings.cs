// ============================================================================
// S E T T I N G S
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

// NOTE: Callers should be aware of some interesting ownership details. This class doesn't
// actually allocate storage for values. The storage comes from the caller during set,
// so objects that can be referenced WILL be referenced AND WILL CHANGE when the caller
// changes them. (most notably, string arrays). If you don't want this, send in a clone 
// of the object instead
namespace TCore.Settings
{
	public partial class Settings //STE
	{
		#region Types

		public enum Type
		{
			Bool = 1,
			Str = 2,
			Int = 3,
			Dttm = 4,
			StrArray = 5
		};

		#endregion

		#region Data model

		private SettingsElt[] m_rgstee;
		private string m_sRoot;
		private string m_sTag;

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
					if (rehe.type == Type.Dttm)
					{
						m_rgstee[i].oref = dttmVal;
					}
					else
					{
						m_rgstee[i].oref = (string) sVal;
					}
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

		public void SetRgsValueFromEnumerable(string sKey, IEnumerable<string> iens)
		{
			string[] rgs = new string[iens.Count()];

			int i = 0;
			foreach (string s in iens)
				rgs[i++] = s;

			SetRgsValue(sKey, rgs);
		}

		public void SetSValue(string sKey, string sValue)
		{
			int i = IFindKey(sKey);
			if (i != -1)
				m_rgstee[i].oref = sValue;
			else
				throw new Exception("could not find given key");
		}

		public void SetNValue(string sKey, string sNValue)
		{
			SetNValue(sKey, sNValue == null ? 0 : Int32.Parse(sNValue));
		}

		public void SetNValue(string sKey, int nValue)
		{
			int i = IFindKey(sKey);
			if (i != -1)
				m_rgstee[i].oref = nValue;
			else
				throw new Exception("could not find given key");
		}

		public void SetDttmValue(string sKey, DateTime dttm)
		{
			int i = IFindKey(sKey);
			if (i != -1)
				m_rgstee[i].oref = dttm;
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
			object o = OFindValue(sKey);

			if (o is string s)
			{
				if (String.IsNullOrEmpty(s))
					return DateTime.MinValue;

				return DateTime.Parse(s);
			}

			return (DateTime) o;
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

		/*----------------------------------------------------------------------------
			%%Function: CreateSettings
			%%Qualified: TCore.Settings.Settings.CreateSettings<T>

			Create a new settings object for the annotated type T
		----------------------------------------------------------------------------*/
		public static Settings CreateSettings<T>(string sReRoot, string sTag)
		{
			return new Settings(SettingsElt.CreateSettings<T>(), sReRoot, sTag);
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

		#region Synchronization

		static T GetSettingValueFromProperty<T>(PropertyInfo propertyInfo, Object o)
		{
			if (propertyInfo == null)
				throw new Exception("cannot derive type information");

			return (T) propertyInfo.GetValue(o);
		}

		static T GetSettingValueFromField<T>(FieldInfo fieldInfo, Object o)
		{
			if (fieldInfo == null)
				throw new Exception("cannot derive type information");

			return (T) fieldInfo.GetValue(o);
		}

		static T GetSettingValue<T>(TypeInfo typeInfo, MemberInfo memberInfo, Object o)
		{
			if (memberInfo.MemberType == MemberTypes.Property)
				return GetSettingValueFromProperty<T>(typeInfo.GetDeclaredProperty(memberInfo.Name), o);

			if (memberInfo.MemberType == MemberTypes.Field)
				return GetSettingValueFromField<T>(typeInfo.GetDeclaredField(memberInfo.Name), o);

			throw new Exception("cannot derive type");
		}

		static T GetValueFromObject<T>(TypeInfo typeInfo, MemberInfo memberInfo, Object settingsClient)
		{
			Settings.Type typeLeft = GetSettingType(typeInfo, memberInfo);
			Settings.Type typeRight = GetSettingTypeFromTypeName(typeof(T).FullName);

			if (typeLeft != typeRight)
			{
				throw new Exception(
					$"cannot get value, types {typeLeft} != {typeRight}");
			}

			return GetSettingValue<T>(typeInfo, memberInfo, settingsClient);
		}

		static void SetSettingValueFromProperty<T>(PropertyInfo propertyInfo, Object o, T value)
		{
			if (propertyInfo == null)
				throw new Exception("cannot derive type information");

			propertyInfo.SetValue(o, value);
		}

		static void SetSettingValueFromField<T>(FieldInfo fieldInfo, Object o, T value)
		{
			if (fieldInfo == null)
				throw new Exception("cannot derive type information");

			fieldInfo.SetValue(o, value);
		}

		static void SetSettingValue<T>(TypeInfo typeInfo, MemberInfo memberInfo, Object o, T value)
		{
			if (memberInfo.MemberType == MemberTypes.Property)
				SetSettingValueFromProperty<T>(typeInfo.GetDeclaredProperty(memberInfo.Name), o, value);
			else if (memberInfo.MemberType == MemberTypes.Field)
				SetSettingValueFromField<T>(typeInfo.GetDeclaredField(memberInfo.Name), o, value);
			else
				throw new Exception("cannot derive type");
		}

		static void SetValueFromObject<T>(TypeInfo typeInfo, MemberInfo memberInfo, Object settingsClient, T value)
		{
			if (GetSettingType(typeInfo, memberInfo) != GetSettingTypeFromTypeName(typeof(T).FullName))
				throw new Exception(
					$"cannot get value, types {GetSettingType(typeInfo, memberInfo)} != {GetSettingTypeFromTypeName(typeof(T).FullName)}");

			SetSettingValue<T>(typeInfo, memberInfo, settingsClient, value);
		}

		/*----------------------------------------------------------------------------
			%%Function: SynchronizeSetValues
			%%Qualified: TCore.Settings.Settings.SynchronizeSetValues<T>

			Synchronize the Settings object with the client Settings.

			Client Settings => Settings
		----------------------------------------------------------------------------*/
		public void SynchronizeSetValues<T>(T settingsClient)
		{
			TypeInfo typeInfo = typeof(T).GetTypeInfo();

			foreach (MemberInfo member in typeInfo.DeclaredMembers)
			{
				SettingAttribute attrInfo = GetSettingAttribute(member);
				if (attrInfo == null)
					continue;

				string key = attrInfo.Key ?? member.Name;
				Settings.Type type = attrInfo.SettingsType ?? GetSettingType(typeInfo, member);

				switch (type)
				{
					case Settings.Type.Int:
						SetNValue(key, GetValueFromObject<int>(typeInfo, member, settingsClient));
						break;
					case Settings.Type.Str:
						SetSValue(key, GetValueFromObject<string>(typeInfo, member, settingsClient));
						break;
					case Settings.Type.Dttm:
						SetDttmValue(key, GetValueFromObject<DateTime>(typeInfo, member, settingsClient));
						break;
					case Settings.Type.Bool:
						SetFValue(key, GetValueFromObject<bool>(typeInfo, member, settingsClient));
						break;
					case Settings.Type.StrArray:
						SetRgsValue(key, GetValueFromObject<string[]>(typeInfo, member, settingsClient));
						break;
					default:
						throw new Exception("cannot synchronize unknown value type");
				}
			}
		}

		/*----------------------------------------------------------------------------
			%%Function: SynchronizeGetValues
			%%Qualified: TCore.Settings.Settings.SynchronizeGetValues<T>
		
			Synchronize the Settings object with the client Settings.

			Client Settings <= Settings			
		----------------------------------------------------------------------------*/
		public void SynchronizeGetValues<T>(T settingsClient)
		{
			TypeInfo typeInfo = typeof(T).GetTypeInfo();

			foreach (MemberInfo member in typeInfo.DeclaredMembers)
			{
				SettingAttribute attrInfo = GetSettingAttribute(member);
				if (attrInfo == null)
					continue;

				string key = attrInfo.Key ?? member.Name;
				Settings.Type type = attrInfo.SettingsType ?? GetSettingType(typeInfo, member);

				switch (type)
				{
					case Settings.Type.Int:
						SetValueFromObject<int>(typeInfo, member, settingsClient, NValue(key));
						break;
					case Settings.Type.Str:
						SetValueFromObject<string>(typeInfo, member, settingsClient, SValue(key));
						break;
					case Settings.Type.Dttm:
						SetValueFromObject<DateTime>(typeInfo, member, settingsClient, DttmValue(key));
						break;
					case Settings.Type.Bool:
						SetValueFromObject<bool>(typeInfo, member, settingsClient, FValue(key)); 
						break;
					case Settings.Type.StrArray:
						SetValueFromObject<string[]>(typeInfo, member, settingsClient, RgsValue(key));
						break;
					default:
						throw new Exception("cannot synchronize unknown value type");
				}
			}
		}

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

			if (oref == null)
				return null;

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
			else if (oref is string || oref is Int32 || oref is DateTime || oref is bool || oref is Int16 ||
			         oref is string[])
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
		public static string[] RgsGetSubkeys(string sRegRoot, bool fEnsureExists = false)
		{
			RegistryKey rk = Registry.CurrentUser.OpenSubKey(sRegRoot);

			if (rk == null && fEnsureExists)
				rk = Registry.CurrentUser.CreateSubKey(sRegRoot);

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
