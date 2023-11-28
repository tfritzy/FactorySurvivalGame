using UnityEngine;
using UnityEngine.UIElements;

public class GradientElement : VisualElement
{
    static readonly Vertex[] _vertices = new Vertex[5];
    static readonly ushort[] _indices = { 0, 1, 2, 2, 3, 0 };

    Color c1;
    Color c2;

    public GradientElement(Color c1, Color c2)
    {
        generateVisualContent += GenerateVisualContent;
        this.c1 = c1;
        this.c2 = c2;
    }

    public void GenerateVisualContent(MeshGenerationContext meshGenerationContext)
    {
        var rect = contentRect;
        if (rect.width < 0.1f || rect.height < 0.1f)
            return;

        UpdateVerticesTint();
        UpdateVerticesPosition(rect);

        var meshWriteData = meshGenerationContext.Allocate(_vertices.Length, _indices.Length);
        meshWriteData.SetAllVertices(_vertices);
        meshWriteData.SetAllIndices(_indices);
    }

    static void UpdateVerticesPosition(Rect rect)
    {
        const float left = 0f;
        var right = rect.width;
        const float top = 0f;
        var bottom = rect.height;
        var center = new Vector3((left + right) * 0.5f, (top + bottom) * 0.5f, Vertex.nearZ);

        _vertices[0].position = new Vector3(left, bottom, Vertex.nearZ);
        _vertices[1].position = new Vector3(left, top, Vertex.nearZ);
        _vertices[2].position = new Vector3(right, top, Vertex.nearZ);
        _vertices[3].position = new Vector3(right, bottom, Vertex.nearZ);
        _vertices[4].position = center;
    }

    void UpdateVerticesTint()
    {
        _vertices[0].tint = c1;
        _vertices[1].tint = c2;
        _vertices[2].tint = c1;
        _vertices[3].tint = c2;
        _vertices[4].tint = c2;
    }
}
