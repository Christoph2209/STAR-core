using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private float lineWidth = 0.2f;
    [SerializeField]
    private Color color  = new(1f, 0.8f, 0.3f, 0.5f);


    private const float ThetaScale = 0.01f;
    private int Size;
    private float Theta;
    private LineRenderer lineRenderer;
    private Material mat;

    public static GameObject Create(Transform parent, Vector3 position, Quaternion rotation,  float radius, float lineWidth, Color color )
    {
        GameObject newCircle = new GameObject("Circle");
        newCircle.transform.parent = parent;
        newCircle.transform.position = position;
        newCircle.transform.rotation = rotation; 
        DrawCircle drawCircle = newCircle.AddComponent<DrawCircle>();

        drawCircle.radius = radius;
        drawCircle.lineWidth = lineWidth;
        drawCircle.color = color;

        drawCircle.ReDraw();

        return newCircle;
    }

    void Awake()
    {
        mat = new(Shader.Find("Universal Render Pipeline/Unlit"));
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = mat;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        ReDraw();
    }


    public void ReDraw()
    {
        mat.color = color;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        lineRenderer.positionCount = Size;

        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
