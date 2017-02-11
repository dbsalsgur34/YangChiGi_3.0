using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRazor : MonoBehaviour {

    float theta;

    IEnumerator DrawCircle(Vector3 pos1, float radius)
    {
        theta = 0;
        float vertexCount;
        if (radius < 6)
        {
            vertexCount = 2 * Mathf.PI * radius * 2;
        }
        else
        {
            vertexCount = 2 * Mathf.PI * radius / 2;
        }

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Color c1 = Color.red;

        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.05f, 0.05f);
        lineRenderer.SetVertexCount((int)vertexCount + 1);

        float deltaTheta = (2.0f * Mathf.PI) / vertexCount;

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
