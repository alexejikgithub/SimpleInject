using UnityEngine;

public class AutoInjector : MonoBehaviour
{

	bool _hasInjected;


	public void Awake()
	{
		_hasInjected = true;
		GetContainerForCurrentScene().InjectGameObject(gameObject);
	}


	private DiContainer GetContainerForCurrentScene()
	{
		return SceneContext.Instance.Container;
	}


}
