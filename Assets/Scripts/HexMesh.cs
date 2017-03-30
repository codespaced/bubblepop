using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{

    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;
    //private MeshCollider meshCollider;
    List<Color> colors;
    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        //meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(Bubble[] bubbles)
    {
        //hexMesh.Clear();
        //vertices.Clear();
        //colors.Clear();
        //triangles.Clear();
        for (int i = 0; i < bubbles.Length; i++)
        {
            Triangulate(bubbles[i]);
        }
        //hexMesh.vertices = vertices.ToArray();
        //hexMesh.colors = colors.ToArray();
        //hexMesh.triangles = triangles.ToArray();
        //hexMesh.RecalculateNormals();

        //meshCollider.sharedMesh = hexMesh;
    }

    void Triangulate(Bubble bubble)
    {
        for (HexDirection d = HexDirection.N; d <= HexDirection.NW; d++)
        {
            Triangulate(d, bubble);
        }
    }

    void Triangulate(HexDirection direction, Bubble bubble) {
        //Vector3 center = bubble.transform.localPosition;
        //AddTriangle(
        //    center,
        //    center + HexMetrics.GetFirstCorner(direction),
        //    center + HexMetrics.GetSecondCorner(direction)
        //);
        //Bubble neighbor = bubble.GetNeighbor(direction) ?? bubble;
        //Color edgeColor = (bubble.color + neighbor.color) * 0.5f;
        //AddTriangleColor(bubble.color, edgeColor, edgeColor);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}