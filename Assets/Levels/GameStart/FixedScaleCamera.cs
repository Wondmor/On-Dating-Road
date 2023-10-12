using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedScaleCamera : MonoBehaviour
{
    Camera camera = null;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null)
        {
            Camera.main.aspect = 16.0f / 9.0f;
        }
    }
}
