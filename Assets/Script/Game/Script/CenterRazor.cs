using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRazor : MonoBehaviour {

    private float radius;
    private LineRenderer lineRenderer;
    private Color lineColor;
    private float vertexCount;
    private void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.startColor = lineColor; lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.25f; lineRenderer.endWidth = 0.25f;
        lineColor = Color.red;
        radius = 26;

        if (radius < 6)
        {
            vertexCount = 2 * Mathf.PI * radius * 2;
        }
        else
        {
            vertexCount = 2 * Mathf.PI * radius / 2;
        }
    }

    IEnumerator DrawCircle(Vector3 pos1, float radius)
    {
        float theta = 0;

        float deltaTheta = (1.0f * Mathf.PI) / vertexCount;

        for (int i = 0; i < (int)vertexCount + 1; i++)
        {
            float x = radius * Mathf.Cos(theta) + (pos1.x - transform.position.x);
            float y = transform.position.y - pos1.y + 0.1f;
            float z = radius * Mathf.Sin(theta) + (pos1.z - transform.position.z);
            Vector3 pos = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;

        }
        yield return null;
    }
}
