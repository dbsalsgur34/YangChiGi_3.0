using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    public Vector3 pos;
    public float value;
    float theta;

    void DrawCircle(Vector3 pos1, float radius, float maxtheta)
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
        Color c1 = Color.blue;

        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.05f, 0.05f);
        lineRenderer.SetVertexCount((int)vertexCount + 1);

        float deltaTheta = (2.0f * Mathf.PI * maxtheta / 360) / (vertexCount);

        for (int i = 0; i < (vertexCount); i++)
        {
            float x = radius * Mathf.Cos(theta) + (pos1.x - transform.position.x);
            float y = transform.position.y - pos1.y + 0.1f;
            float z = radius * Mathf.Sin(theta) + (pos1.z - transform.position.z);
            Vector3 pos = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;

        }
    }

    void DrawThrowingArc(Vector3 pos1, Vector3 pos2, float coverValue)
    {
        Color hitlineColor;
        hitlineColor.r = coverValue / 100;
        hitlineColor.g = 1.0f - (coverValue / 100);
        hitlineColor.b = 0.0f;
        hitlineColor.a = 0.5f;

        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        float sections = 5.0f + Mathf.Round(Vector3.Distance(pos1, pos2));

        lineRenderer.SetVertexCount((int)sections);

        float t;
        for (int i = 0; i < (int)sections; i++)
        {
            t = i / (sections - 1);
            lineRenderer.SetPosition(i, GetQuadraticCoordinates(t, pos1 + Vector3.up * 1.4f, Vector3.Lerp(pos1, pos2, 0.5f) + Vector3.up * (Vector3.Distance(pos1, pos2) / 2), pos2 + Vector3.up * 0.1f));
        }

        lineRenderer.SetWidth(0.05f, 0.05f);
        lineRenderer.SetColors(hitlineColor, hitlineColor);

        lineRenderer.enabled = true;
    }

    Vector3 GetQuadraticCoordinates(float t, Vector3 p0, Vector3 c0, Vector3 p1)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * t * (1 - t) * c0 + Mathf.Pow(t, 2) * p1;
    }
}
