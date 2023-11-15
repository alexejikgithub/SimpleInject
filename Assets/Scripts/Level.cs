using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Inject] private Cube _cube;
    void Awake()
    {
        _cube.EnableRotation();
	}

    
}
