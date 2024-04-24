using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MousePosition2D : MonoBehaviour
{
    public void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
