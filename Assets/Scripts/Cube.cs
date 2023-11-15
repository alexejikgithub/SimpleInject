using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	private bool _isRotating;

	void Update()
	{
		if (!_isRotating)
			return;
		transform.Rotate(0, 0, 50 * Time.deltaTime); //rotates 50 degrees per second around z axis
	}

	public void EnableRotation()
	{
		_isRotating = true;
	}
}
