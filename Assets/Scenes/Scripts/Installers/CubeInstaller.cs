using UnityEngine;

public class CubeInstaller : MonoInstaller
{
	[SerializeField] private Cube _cube;

	public override void InstallBindings()
	{
		_container.Bind<Cube>(_cube);
	}
}