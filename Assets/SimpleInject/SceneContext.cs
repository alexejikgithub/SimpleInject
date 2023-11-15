using UnityEngine;

public class SceneContext : MonoBehaviour
{

	[SerializeField] private MonoInstaller[] _monoinstallers;

	private static SceneContext _instance;
	private DiContainer _diContaier;

	public static SceneContext Instance => _instance;
	public DiContainer Container => _diContaier;

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
			monoInstaller.InstallBindings(_diContaier);
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}
}
