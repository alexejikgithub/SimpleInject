using System;
using System.Collections.Generic;
using System.Linq;

public static class TypeAnalyzer
{
	static Dictionary<Type, InjectTypeInfo> _typeInfo = new Dictionary<Type, InjectTypeInfo>();

	public static InjectTypeInfo TryGetInfo(Type type)
	{
		InjectTypeInfo info;

		if (_typeInfo.TryGetValue(type, out info))
		{
			return info;
		}

		info = GetInfoInternal(type);
		if (info != null)
		{
			var baseType = type.BaseType();

			if (baseType != null && !ShouldSkipTypeAnalysis(baseType))
			{
				info.BaseTypeInfo = TryGetInfo(baseType);
			}
		}

		_typeInfo.Add(type, info);

		return info;
	}

	static InjectTypeInfo GetInfoInternal(Type type)
	{
		if (ShouldSkipTypeAnalysis(type))
		{
			return null;
		}

		return CreateTypeInfoFromReflection(type);
	}

	public static bool ShouldSkipTypeAnalysis(Type type) // TODO Add more exceptions?
	{
		return type == null || type.IsEnum() || type.IsArray || type.IsInterface()
			|| type.ContainsGenericParameters() || type.IsStaticType()
			|| type == typeof(object);
	}

	static InjectTypeInfo CreateTypeInfoFromReflection(Type type)
	{
		var reflectionInfo = ReflectionTypeAnalyzer.GetReflectionInfo(type);

		var memberInfos = reflectionInfo.InjectFields.Select(
			x => ReflectionInfoTypeInfoConverter.ConvertField(x)).ToArray();

		return new InjectTypeInfo(memberInfos);
	}
}