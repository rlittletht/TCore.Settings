using System;
using System.Collections.Generic;
using System.Reflection;

namespace TCore.Settings
{
	public partial class Settings
	{
		public class SettingsElt
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

			/*----------------------------------------------------------------------------
				%%Function: Clone
				%%Qualified: TCore.Settings.Settings.SettingsElt.Clone

				Clone a single settings element
			----------------------------------------------------------------------------*/
			public SettingsElt Clone()
			{
				return new SettingsElt(sRePath, type, oref, oDefault);
			}

			static bool IsEqualObject(object oLeft, object oRight)
			{
				if (oLeft is bool)
				{
					if (!(oRight is bool))
						return false;

					if ((bool) oLeft != (bool) oRight)
						return false;
				}

				if (oLeft is int)
				{
					if (!(oRight is int))
						return false;

					if ((int)oLeft != (int)oRight)
						return false;
				}

				if (oLeft is DateTime)
				{
					if (!(oRight is DateTime))
						return false;

					if ((DateTime)oLeft != (DateTime)oRight)
						return false;
				}

				if (oLeft is string)
				{
					if (!(oRight is string))
						return false;

					if (String.Compare((string)oRight, (string)oLeft) != 0)
						return false;
				}

				return true;
			}

			public bool IsEqual(SettingsElt elt)
			{
				if (String.Compare(elt.sRePath, this.sRePath) != 0)
					return false;

				if (elt.type != this.type)
					return false;

				if (!IsEqualObject(elt.oref, this.oref))
					return false;

				if (!IsEqualObject(elt.oDefault, this.oDefault))
					return false;

				return true;
			}
			#region Construction

			/*----------------------------------------------------------------------------
				%%Function: CreateSettings
				%%Qualified: TCore.Settings.Settings.SettingsElt.CreateSettings<T>

				Create an array of settings based on the type T. Collects information
				from the [Setting()] attributes on the type
			----------------------------------------------------------------------------*/
			public static SettingsElt[] CreateSettings<T>()
			{
				TypeInfo typeInfo = typeof(T).GetTypeInfo();

				List<SettingsElt> settings = new List<SettingsElt>();

				foreach (MemberInfo member in typeInfo.DeclaredMembers)
				{
					SettingAttribute attrInfo = GetSettingAttribute(member);
					if (attrInfo == null)
						continue;

					string sKey = attrInfo.Key ?? member.Name;
					Settings.Type type = attrInfo.SettingsType ?? GetSettingType(typeInfo, member);

					SettingsElt setting = new SettingsElt(
						sKey,
						type,
						attrInfo.ModelSourceOrInitialValue,
						attrInfo.DefaultRegistryValue);

					settings.Add(setting);
				}

				return settings.ToArray();
			}
			#endregion
		}

		static SettingAttribute GetSettingAttribute(MemberInfo memberInfo)
		{
			foreach (SettingAttribute settingAttr in memberInfo.GetCustomAttributes<SettingAttribute>(false))
				return settingAttr; // we just want the first one

			return null;
		}

		static Settings.Type GetSettingTypeFromTypeName(string typeName)
		{
			switch (typeName)
			{
				case "System.Int32":
					return Settings.Type.Int;
				case "System.Boolean":
					return Settings.Type.Bool;
				case "System.String":
					return Settings.Type.Str;
				case "System.DateTime":
					return Settings.Type.Dttm;
				case "System.String[]":
					return Settings.Type.StrArray;
				default:
					throw new Exception("could not derive type");
			}
		}

		static Settings.Type GetSettingTypeFromProperty(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
				throw new Exception("cannot derive type information");

			return GetSettingTypeFromTypeName((propertyInfo.PropertyType.FullName));
		}

		static Settings.Type GetSettingTypeFromField(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
				throw new Exception("cannot derive type information");

			return GetSettingTypeFromTypeName(fieldInfo.FieldType.FullName);
		}

		static Settings.Type GetSettingType(TypeInfo typeInfo, MemberInfo memberInfo)
		{
			if (memberInfo.MemberType == MemberTypes.Property)
				return GetSettingTypeFromProperty(typeInfo.GetDeclaredProperty(memberInfo.Name));

			if (memberInfo.MemberType == MemberTypes.Field)
				return GetSettingTypeFromField(typeInfo.GetDeclaredField(memberInfo.Name));

			throw new Exception("cannot derive type");
		}
	}
}