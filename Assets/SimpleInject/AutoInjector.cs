using UnityEngine;

public class AutoInjector : MonoBehaviour
{
	public void Awake()
	{
		InjectGameObject();
	}

	private void InjectGameObject()
	{
		GetContainer().InjectGameObject(gameObject);
	}

	private DiContainer GetContainer()
	{
		return SceneContext.Instance.Container;
	}
}
