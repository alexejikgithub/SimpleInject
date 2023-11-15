using System;
using UnityEngine;


public abstract class MonoInstallerBase : MonoBehaviour, IInstaller
{
	public virtual void InstallBindings(DiContainer container)
	{
	}
}


