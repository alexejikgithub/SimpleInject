using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if !NOT_UNITY3D
using UnityEngine;
#endif


public static class ReflectionTypeAnalyzer
{
	static readonly HashSet<Type> _injectAttributeTypes;

	static ReflectionTypeAnalyzer()
	{
		_injectAttributeTypes = new HashSet<Type>();
		_injectAttributeTypes.Add(typeof(InjectAttributeBase));
	}


	public static ReflectionTypeInfo GetReflectionInfo(Type type)
	{
		if (type.IsEnum())
		{
			Debug.LogWarning(("Tried to analyze enum type '{0}'.  This is not supported", type));
		}

		if (type.IsArray)
		{
			Debug.LogWarning(("Tried to analyze array type '{0}'.  This is not supported", type));
		}



		var baseType = type.BaseType();

		if (baseType == typeof(object))
		{
			baseType = null;
		}

		return new ReflectionTypeInfo(
			type, baseType,
			GetFieldInfos(type));
	}


	static List<ReflectionTypeInfo.InjectFieldInfo> GetFieldInfos(Type type)
	{
		return type.DeclaredInstanceFields()
			.Where(x => _injectAttributeTypes.Any(a => x.HasAttribute(a)))
			.Select(x => new ReflectionTypeInfo.InjectFieldInfo(
				x, GetInjectableInfoForMember(type, x)))
			.ToList();
	}



	static InjectableInfo GetInjectableInfoForMember(Type parentType, MemberInfo memInfo)
	{
		var injectAttributes = memInfo.AllAttributes<InjectAttributeBase>().ToList();

		if (!(injectAttributes.Count <= 1))
		{
			Debug.LogWarning(("Found multiple 'Inject' attributes on type field '{0}' of type '{1}'.  Field should only container one Inject attribute", memInfo.Name, parentType));
		}


		var injectAttr = injectAttributes.SingleOrDefault();

		object identifier = null;
		bool isOptional = false;
		InjectSources sourceType = InjectSources.Any;

		if (injectAttr != null)
		{
			identifier = injectAttr.Id;
			isOptional = injectAttr.Optional;
			sourceType = injectAttr.Source;
		}

		Type memberType = memInfo is FieldInfo
			? ((FieldInfo)memInfo).FieldType : ((PropertyInfo)memInfo).PropertyType;

		return new InjectableInfo(
			isOptional,
			identifier,
			memInfo.Name,
			memberType,
			null,
			sourceType);
	}


}
