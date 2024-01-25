using UnityEditor;
using UnityEngine;


public class MeshSaverEditor : MonoBehaviour
{
    [ContextMenu("Save Mesh")]
    public void SaveMesh()
    {
        Mesh mesh = CreateTriangle();  // or however you get your mesh
        AssetDatabase.CreateAsset(mesh, "Assets/Triangle.asset");
        AssetDatabase.SaveAssets();
    }

    Mesh CreateTriangle()
    {
        Mesh m = new Mesh();
        m.vertices = new Vector3[]
        {
            new Vector3(0, 0.5f, 0), // Top vertex
            new Vector3(-0.5f, -0.5f, 0), // Bottom-left vertex
            new Vector3(0.5f, -0.5f, 0) // Bottom-right vertex
        };
        m.triangles = new int[] {  0, 2, 1 };

        return m;
    }
}