using UnityEngine;

public class SceneContext : MonoBehaviour
{

	[SerializeField] private MonoInstaller[] _monoinstallers;

	private DiContainer _diContaier;

	public DiContainer Container => _diContaier; 


	private static SceneContext _instance;

	public static SceneContext Instance => _instance;


	private void Awake()
	{
		_instance = this;
		_diContaier = new DiContainer();
		InstallInstallers();
	}

	private void InstallInstallers()
	{
		foreach(var monoInstaller in _monoinstallers)
		{
			monoInstaller.SetContainer(_diContaier);
			monoInstaller.InstallBindings();
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}
}
