
using System;
using System.Collections.Generic;
using UnityEngine;

public class DiContainer
{

	private Dictionary<Type, object> _installations;

	public DiContainer()
	{
		_installations = new Dictionary<Type, object>();
	}

	public void InjectGameObject(GameObject gameObject)
	{

		var monoBehaviours = new List<MonoBehaviour>(gameObject.GetComponents<MonoBehaviour>()); //TODO Change to Pool

		for (int i = 0; i < monoBehaviours.Count; i++)
		{
			
			Inject(monoBehaviours[i]);
		}

	}

	public void Inject(object injectable)
	{
		Type injectableType;

		
		injectableType = injectable.GetType();
		
		InjectExplicitInternal(
			injectable,
			injectableType);
	}


	void InjectExplicitInternal(
	object injectable, Type injectableType)
	{

		if (injectable == null)
		{
			Debug.LogWarning(
				"Tinjectable == null");
			return;
		}

		var typeInfo = TypeAnalyzer.TryGetInfo(injectableType);

		if (typeInfo == null)
		{
			Debug.LogWarning(
				"Typeinfo is null");
			return;
		}


		if (injectableType == typeof(GameObject))
		{
			Debug.LogWarning(
				"Use InjectGameObject to Inject game objects instead of Inject method"
				);
		}



		InjectMembersTopDown(
			injectable, injectableType, typeInfo);


	}



	void InjectMembersTopDown(
			object injectable, Type injectableType,
			InjectTypeInfo typeInfo)
	{

		if (typeInfo.BaseTypeInfo != null)
		{
			InjectMembersTopDown(
				injectable, injectableType, typeInfo.BaseTypeInfo);
		}

		for (int i = 0; i < typeInfo.InjectMembers.Length; i++)
		{
			
			var injectInfo = typeInfo.InjectMembers[i].Info;
			var setterMethod = typeInfo.InjectMembers[i].Setter;

			object value = _installations.GetValueOrDefault(injectInfo.MemberType);

			Debug.Log(injectInfo.MemberType);


			// Получаем все ключи и значения
			Console.WriteLine("\nKeys and Values:");
			foreach (var pair in _installations)
			{
				Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
			}

			setterMethod(injectable, value);

		}
	}



	public void Bind<TContract>(TContract monobehaviour)
	{

		
		Type type = typeof(TContract);

		if (_installations.TryAdd(type, monobehaviour))
		{
			// Элемент успешно добавлен
			Debug.Log($"Successfully added {type.Name} to the dictionary.");
		}
		else
		{
			// Элемент с таким ключом уже существует
			Debug.LogWarning($"{type.Name} is already in the dictionary.");
		}
	}

}


