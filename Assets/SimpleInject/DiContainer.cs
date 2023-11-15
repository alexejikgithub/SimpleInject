using System;
using System.Collections.Generic;
using UnityEngine;

public class DiContainer
{
	private Dictionary<Type, object> _bindings;
	public DiContainer()
	{
		_bindings = new Dictionary<Type, object>();
	}

	public void InjectGameObject(GameObject gameObject)
	{
		var monoBehaviours = new List<MonoBehaviour>(gameObject.GetComponents<MonoBehaviour>()); //TODO Change to Pool?

		for (int i = 0; i < monoBehaviours.Count; i++)
		{
			Inject(monoBehaviours[i]);
		}
	}

	public void Inject(object injectable)
	{
		Type injectableType;

		injectableType = injectable.GetType();

		InjectExplicitInternal(injectable, injectableType);
	}

	void InjectExplicitInternal(object injectable, Type injectableType)
	{
		if (injectable == null)
		{
			Debug.LogWarning("Tinjectable == null");
			return;
		}

		var typeInfo = TypeAnalyzer.TryGetInfo(injectableType);

		if (typeInfo == null)
		{
			Debug.LogWarning("Typeinfo is null");
			return;
		}

		if (injectableType == typeof(GameObject))
		{
			Debug.LogWarning("Gameobjects cannot be Injected");
		}

		InjectMembersTopDown(injectable, injectableType, typeInfo);
	}

	void InjectMembersTopDown(object injectable, Type injectableType, InjectTypeInfo typeInfo)
	{

		if (typeInfo.BaseTypeInfo != null)
		{
			InjectMembersTopDown(injectable, injectableType, typeInfo.BaseTypeInfo);
		}

		for (int i = 0; i < typeInfo.InjectMembers.Length; i++)
		{
			var injectInfo = typeInfo.InjectMembers[i].Info;
			var setterMethod = typeInfo.InjectMembers[i].Setter;

			object value = GetBinding(injectInfo.MemberType);
			setterMethod(injectable, value);
		}
	}

	public void Bind<TContract>(TContract monobehaviour)
	{
		Type type = typeof(TContract);

		if (!_bindings.TryAdd(type, monobehaviour))
		{
			Debug.LogWarning($"{type.Name} is already in the dictionary.");
		}
	}

	public void AddInstallation(Type type, object installation)
	{
		_bindings[type] = installation;
	}

	public object GetBinding(Type type)
	{
		if (_bindings.TryGetValue(type, out object installation))
		{
			return installation;
		}
		else
		{
			Debug.LogWarning($"No binding found for {type.Name}");
			return null;
		}
	}
}