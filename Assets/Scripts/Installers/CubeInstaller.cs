using UnityEngine;

public class CubeInstaller : MonoInstaller
{
	[SerializeField] private Cube _cube;

	public override void InstallBindings(DiContainer container)
	{
		container.Bind(_cube);
	}
}