// Grid Background
using UnityEngine;
using UnityEngine.UI;

public class GridBackground : Graphic
{
    [Header("Grid Settings")]
    public Color gridColor = new Color(1f, 1f, 1f, 0.2f);
    public float cellSize = 60f;
    public float thickness = 2f; // This is a slider

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        // Get the actual pixel dimensions of the RectTransform
        Rect r = rectTransform.rect;

        // Draw Vertical Lines
        for (float x = r.xMin; x <= r.xMax; x += cellSize)
        {
            DrawLine(vh, new Vector2(x, r.yMin), new Vector2(x, r.yMax));
        }

        // Draw Horizontal Lines
        for (float y = r.yMin; y <= r.yMax; y += cellSize)
        {
            DrawLine(vh, new Vector2(r.xMin, y), new Vector2(r.xMax, y));
        }
    }

        void DrawLine(VertexHelper vh, Vector2 start, Vector2 end)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x) * (thickness / 2f);

        int count = vh.currentVertCount;

        UIVertex v = UIVertex.simpleVert;
        
        // This line is the secret! It uses the color from the Inspector
        v.color = gridColor; 

        v.position = start - perp; vh.AddVert(v);
        v.position = start + perp; vh.AddVert(v);
        v.position = end + perp;   vh.AddVert(v);
        v.position = end - perp;   vh.AddVert(v);

        vh.AddTriangle(count, count + 1, count + 2);
        vh.AddTriangle(count, count + 2, count + 3);
    }

}