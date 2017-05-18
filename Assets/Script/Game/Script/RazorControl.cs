using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorControl : MonoBehaviour {

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

    public void DrawCircle(Vector3 center, Vector3 HQ, Vector3 target, int RazorPoint)
    {
       
        lineRenderer.positionCount = ((int)vertexCount + 1);

        float theta = 0;
        float maxtheta = (target == Vector3.zero) ? 0 : Vector3.Angle(HQ - center, target);
        float deltaTheta = (2.0f * Mathf.PI * maxtheta / 360) / (vertexCount);
        float xzTheta = Mathf.Atan2(target.z - center.z, target.x - center.x);
        for (int i = 0; i < (vertexCount); i++)
        {
            float x = radius * Mathf.Sin(theta) * Mathf.Cos(xzTheta);
            float y = RazorPoint * radius * Mathf.Cos(theta) + (center.y + transform.position.y);
            float z = radius * Mathf.Sin(theta) * Mathf.Sin(xzTheta);
            Vector3 pos = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }

    public void DrawCenterCircle(Vector3 Center, float radius)
    {
        lineRenderer.positionCount = ((int)vertexCount + 1);

        float theta = 0;
        float deltaTheta = (2.0f * Mathf.PI) / vertexCount;

        for (int i = 0; i < (int)vertexCount + 1; i++)
        {
            float x = radius * Mathf.Cos(theta) + (Center.x - transform.position.x);
            float y = transform.position.y - Center.y + 0.1f;
            float z = radius * Mathf.Sin(theta) + (Center.z - transform.position.z);
            Vector3 pos = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }
}
