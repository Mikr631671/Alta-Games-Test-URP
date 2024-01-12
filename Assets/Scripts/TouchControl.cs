using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    public event Action TouchDown;
    public event Action TouchUp;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) TouchDown?.Invoke();
        if (Input.GetMouseButtonUp(0)) TouchUp?.Invoke();
    }
}
