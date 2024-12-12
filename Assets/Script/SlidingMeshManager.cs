using System.Collections.Generic;
using UnityEngine;

public class SlidingMeshManager : MonoBehaviour
{
    
    // A point on the slicing line
    public Vector3 pointOnLine; 
    // The direction of the slicing line
    public Vector3 lineDirection; 
    public GameObject plane; 

    void Start()
    {
        // Start the slicing process when the script runs
        SlicePlane();
    }

    void SlicePlane()
    {
        // Get the MeshFilter component attached to this GameObject
        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        // Get the mesh data from the MeshFilter
        Mesh mesh = meshFilter.mesh;

        // Convert the mesh vertices and triangles into lists for easier manipulation
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);

        // Lists to store new vertices and the indices for the sliced upper and lower parts
        List<Vector3> newVertices = new List<Vector3>();
        List<int> upperTriangles = new List<int>();
        List<int> lowerTriangles = new List<int>();

        // Loop through each triangle in the mesh
        for (int i = 0; i < triangles.Count; i += 3)
        {
            // Get the vertices of the current triangle
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];

            // Determine which side of the line each vertex is on
            bool isV1Above = IsAboveLine(v1);
            bool isV2Above = IsAboveLine(v2);
            bool isV3Above = IsAboveLine(v3);

            // If all vertices are above the slicing line, add the triangle to the upper part
            if (isV1Above && isV2Above && isV3Above)
            {
                AddTriangle(upperTriangles, newVertices, v1, v2, v3);
            }
            // If all vertices are below the slicing line, add the triangle to the lower part
            else if (!isV1Above && !isV2Above && !isV3Above)
            {
                AddTriangle(lowerTriangles, newVertices, v1, v2, v3);
            }
            else
            {
                // Handle slicing when the triangle is intersected by the line
                SliceTriangle(v1, v2, v3, isV1Above, isV2Above, isV3Above, newVertices, upperTriangles, lowerTriangles);
            }
        }

        // Create new mesh objects for the upper and lower sliced parts
        CreateMesh("UpperPlane", upperTriangles, newVertices);
        CreateMesh("LowerPlane", lowerTriangles, newVertices);
    }

    // Determines if a vertex is above the slicing line
    bool IsAboveLine(Vector3 vertex)
    {
        // Calculate the relative position of the vertex to the slicing line
        Vector3 relativePosition = vertex - pointOnLine;
        // Check if the vertex is above the line using the dot product
        return Vector3.Dot(relativePosition, lineDirection) > 0;
    }

    // Adds a triangle to the specified list of triangles
    void AddTriangle(List<int> triangles, List<Vector3> vertices, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // Get the current index where the vertices will be added
        int index = vertices.Count;
        // Add the vertices of the triangle to the vertices list
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        // Add the indices of the new triangle to the triangle list
        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }

    // Placeholder function to handle the slicing of a triangle
    void SliceTriangle(Vector3 v1, Vector3 v2, Vector3 v3, bool isV1Above, bool isV2Above, bool isV3Above, List<Vector3> vertices, List<int> upperTriangles, List<int> lowerTriangles)
    {
        // Logic to handle slicing of the triangle along the line
        // This involves finding intersection points and creating new vertices
        // Detailed implementation depends on the exact needs and geometry
    }

    // Creates a new GameObject with the sliced mesh
    void CreateMesh(string name, List<int> triangles, List<Vector3> vertices)
    {
        // Create a new GameObject to hold the mesh
        GameObject newObject = new GameObject(name);
        // Add a MeshFilter component to hold the mesh data
        MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
        // Add a MeshRenderer component and copy the material from the original
        MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.material = plane.GetComponent<MeshRenderer>().material;

        // Create a new mesh and set its vertices and triangles
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = triangles.ToArray();
        // Recalculate normals for correct lighting
        newMesh.RecalculateNormals();

        // Assign the new mesh to the MeshFilter component
        meshFilter.mesh = newMesh;
    }
}
