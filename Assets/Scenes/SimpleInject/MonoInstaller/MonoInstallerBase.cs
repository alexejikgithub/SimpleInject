using System;
using System.Diagnostics;
using UnityEngine;


// We'd prefer to make this abstract but Unity 5.3.5 has a bug where references
// can get lost during compile errors for classes that are abstract
[DebuggerStepThrough]
public class MonoInstallerBase : MonoBehaviour, IInstaller
{
	[Inject]
	protected DiContainer _container;


	public virtual void InstallBindings()
	{
		throw new NotImplementedException();
	}

	public void SetContainer(DiContainer container)
	{
		_container = container;
	}
}


