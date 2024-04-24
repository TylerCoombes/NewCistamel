using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public int points;
    public float movementSpeed = 1;
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    public void Draw()
    {
        float xStart = 0;
        float Tau = 2 * Mathf.PI;
        float xFinish = Tau;

        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = Mathf.Sin(x) * movementSpeed;
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        Draw();
    }
}
