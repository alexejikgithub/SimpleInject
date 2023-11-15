using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeExtensions
{
	static readonly Dictionary<Type, Type[]> _interfaces = new Dictionary<Type, Type[]>();

	public static bool DerivesFromOrEqual(this Type a, Type b)
	{
		return b == a || b.IsAssignableFrom(a);
	}

	public static bool IsEnum(this Type type)
	{
		return type.IsEnum;
	}

	public static FieldInfo[] DeclaredInstanceFields(this Type type)
	{
		return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
	}

	public static Type BaseType(this Type type)
	{
		return type.BaseType;
	}

	public static bool IsInterface(this Type type)
	{
		return type.IsInterface;
	}

	public static bool ContainsGenericParameters(this Type type)
	{
		return type.ContainsGenericParameters;
	}

	public static bool IsAbstract(this Type type)
	{
		return type.IsAbstract;
	}

	public static bool IsSealed(this Type type)
	{
		return type.IsSealed;
	}
	public static bool IsStaticType(this Type type)
	{
		// Apparently this is unique to static classes
		return type.IsAbstract() && type.IsSealed();
	}

	public static bool HasAttribute(this MemberInfo provider, params Type[] attributeTypes)
	{
		return provider.AllAttributes(attributeTypes).Any();
	}

	public static IEnumerable<T> AllAttributes<T>(this MemberInfo provider) where T : Attribute
	{
		return provider.AllAttributes(typeof(T)).Cast<T>();
	}

	public static IEnumerable<Attribute> AllAttributes(this MemberInfo provider, params Type[] attributeTypes)
	{
		Attribute[] allAttributes;

		allAttributes = System.Attribute.GetCustomAttributes(provider, typeof(Attribute), true);

		if (attributeTypes.Length == 0)
		{
			return allAttributes;
		}

		return allAttributes.Where(a => attributeTypes.Any(x => a.GetType().DerivesFromOrEqual(x)));
	}
}