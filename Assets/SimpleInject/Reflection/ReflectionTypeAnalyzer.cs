using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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

		return new ReflectionTypeInfo(GetFieldInfos(type));
	}

	static List<ReflectionTypeInfo.InjectFieldInfo> GetFieldInfos(Type type)
	{
		return type.DeclaredInstanceFields()
			.Where(x => _injectAttributeTypes.Any(a => x.HasAttribute(a)))
			.Select(x => new ReflectionTypeInfo.InjectFieldInfo(
				x, GetInjectableInfoForMember(type, x)))
			.ToList();
	}

	static InjectableInfo GetInjectableInfoForMember(Type parentType, MemberInfo memberInfo)
	{
		var injectAttributes = memberInfo.AllAttributes<InjectAttributeBase>().ToList();

		if (!(injectAttributes.Count <= 1))
		{
			Debug.LogWarning(("Found multiple 'Inject' attributes on type field '{0}' of type '{1}'.  Field should only container one Inject attribute", memberInfo.Name, parentType));
		}

		Type memberType = ((FieldInfo)memberInfo).FieldType ;

		return new InjectableInfo(memberInfo.Name,memberType);
	}

}
