//#define ZEN_DO_NOT_USE_COMPILED_EXPRESSIONS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if !NOT_UNITY3D
using UnityEngine;
#endif


public static class ReflectionInfoTypeInfoConverter
{

	public static InjectTypeInfo.InjectMemberInfo ConvertField(
		Type parentType, ReflectionTypeInfo.InjectFieldInfo injectField)
	{
		return new InjectTypeInfo.InjectMemberInfo(
			GetSetter(parentType, injectField.FieldInfo), injectField.InjectableInfo);
	}

	
#if !(UNITY_WSA && ENABLE_DOTNET) || UNITY_EDITOR
	static IEnumerable<FieldInfo> GetAllFields(Type t, BindingFlags flags)
	{
		if (t == null)
		{
			return Enumerable.Empty<FieldInfo>();
		}

		return t.GetFields(flags).Concat(GetAllFields(t.BaseType, flags)).Distinct();
	}

	static ZenMemberSetterMethod GetOnlyPropertySetter(
		Type parentType,
		string propertyName)
	{

		if (parentType != null)
		{
			Debug.LogWarning("parentType is null");
		}

		if (string.IsNullOrEmpty(propertyName))
		{
			Debug.LogWarning("propertyName is empty");
		}


		var allFields = GetAllFields(
			parentType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).ToList();

		var writeableFields = allFields.Where(f => f.Name == string.Format("<" + propertyName + ">k__BackingField", propertyName)).ToList();

		if (!writeableFields.Any())
		{
			Debug.LogWarning((
				"Can't find backing field for get only property {0} on {1}.\r\n{2}",
				propertyName, parentType.FullName, string.Join(";", allFields.Select(f => f.Name).ToArray())));
		}

		return (injectable, value) => writeableFields.ForEach(f => f.SetValue(injectable, value));
	}
#endif

	static ZenMemberSetterMethod GetSetter(Type parentType, MemberInfo memInfo)
	{
		var setterMethod = TryGetSetterAsCompiledExpression(parentType, memInfo);

		if (setterMethod != null)
		{
			return setterMethod;
		}

		var fieldInfo = memInfo as FieldInfo;
		var propInfo = memInfo as PropertyInfo;

		if (fieldInfo != null)
		{
			return ((injectable, value) => fieldInfo.SetValue(injectable, value));
		}

		if(propInfo==null)
		{
			Debug.LogWarning("Found null pointer when value was expected");
		}
		

#if UNITY_WSA && ENABLE_DOTNET && !UNITY_EDITOR
            return ((object injectable, object value) => propInfo.SetValue(injectable, value, null));
#else
		if (propInfo.CanWrite)
		{
			return ((injectable, value) => propInfo.SetValue(injectable, value, null));
		}

		return GetOnlyPropertySetter(parentType, propInfo.Name);
#endif
	}

	static ZenMemberSetterMethod TryGetSetterAsCompiledExpression(Type parentType, MemberInfo memInfo)
	{
#if NET_4_6 && !ENABLE_IL2CPP && !ZEN_DO_NOT_USE_COMPILED_EXPRESSIONS

            if (parentType.ContainsGenericParameters)
            {
                return null;
            }

            var fieldInfo = memInfo as FieldInfo;
            var propInfo = memInfo as PropertyInfo;

            // It seems that for readonly fields, we have to use the slower approach below
            // As discussed here: https://www.productiverage.com/trying-to-set-a-readonly-autoproperty-value-externally-plus-a-little-benchmarkdotnet
            // We have to skip value types because those can only be set by reference using an lambda expression
            if (!parentType.IsValueType() && (fieldInfo == null || !fieldInfo.IsInitOnly) && (propInfo == null || propInfo.CanWrite))
            {
                Type memberType = fieldInfo != null
                    ? fieldInfo.FieldType : propInfo.PropertyType;

                var typeParam = Expression.Parameter(typeof(object));
                var valueParam = Expression.Parameter(typeof(object));

                return Expression.Lambda<ZenMemberSetterMethod>(
                    Expression.Assign(
                        Expression.MakeMemberAccess(Expression.Convert(typeParam, parentType), memInfo),
                        Expression.Convert(valueParam, memberType)),
                        typeParam, valueParam).Compile();
            }
#endif

		return null;
	}
}
