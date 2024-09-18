using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceMeshManager : MonoBehaviour
{
     // A point on the slicing line
    
    public GameObject plane;
    public MeshFilter firstSlicedPlane;
    public MeshFilter secondSlicedPlane;
    public Animator cameraAnimator;
    private int hitTimes;
    private Vector3 firstHitPoint;
    private Vector3 secondHitPoint;
    private Vector3 lineDirection; 

    void Start()
    {
        hitTimes = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("cloth"))
            {
                Vector3 worldPosition = hit.point;
                Vector3 localPoint = plane.GetComponent<Transform>().InverseTransformPoint(worldPosition);
                if (hitTimes == 0){
                    
                    firstHitPoint = localPoint;
                    hitTimes++;
                    Debug.Log("Getting first point" + firstHitPoint);
                }
                else if (hitTimes == 1){
                    float distanceToFirstPoint =  Vector3.Distance(firstHitPoint, localPoint);
                    if (distanceToFirstPoint > 0.2){
                        secondHitPoint = localPoint;
                        Debug.Log("Getting second point" + secondHitPoint);
                        lineDirection = secondHitPoint - firstHitPoint;
                        SlicePlane();
                        hitTimes++;
                    }
                }
            }
        }
    }

    void SlicePlane()
    {
        Debug.Log("Start Slicing");
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

        float debugMinVerticeX = 90000f;
        float debugMinVerticeY = 90000f;
        float debugMaxVerticeX = -90000f;
        float debugMaxVerticeY = -90000f;
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

            // Debug.Log("triangle:" + triangles[i] + "vertices" + v1 + "is it above?" + isV1Above);
            // Debug.Log("triangle:" + triangles[i+1] + "vertices" + v2 + "is it above?" + isV2Above);
            // Debug.Log("triangle:" + triangles[i+2] + "vertices" + v3 + "is it above?" + isV3Above);

            // If all vertices are above the slicing line, add the triangle to the upper part
            if (isV1Above && isV2Above && isV3Above)
            {
                Debug.Log("Adding upper vertices 1:" + v1 + "2" + v2 + "3" + v3);
                AddTriangle(upperTriangles, newVertices, v1, v2, v3);
            }
            // If all vertices are below the slicing line, add the triangle to the lower part
            else if (!isV1Above && !isV2Above && !isV3Above)
            {
                Debug.Log("Adding lower vertices 1:" + v1 + "2" + v2 + "3" + v3);
                AddTriangle(lowerTriangles, newVertices, v1, v2, v3);
            }
            else
            {
                Debug.Log("Adding middle vertices 1:" + v1 + "2" + v2 + "3" + v3);
                // Handle slicing when the triangle is intersected by the line
                SliceTriangle(v1, v2, v3, isV1Above, isV2Above, isV3Above, newVertices, upperTriangles, lowerTriangles);
            }
        }
         Debug.Log("minX:" + debugMinVerticeX + "miny" + debugMinVerticeY + "maxX" + debugMaxVerticeX + "maxY" + debugMaxVerticeY);
        // Create new mesh objects for the upper and lower sliced parts
        CreateMesh("UpperPlane", upperTriangles, newVertices, true);
        CreateMesh("LowerPlane", lowerTriangles, newVertices, false);
        MoveCamera();
    }

    // Determines if a vertex is above the slicing line
    bool IsAboveLine(Vector3 vertex)
    {
        //Debug.Log("checking if above line; vertex:" + vertex);
        // Calculate the relative position of the vertex to the slicing line
        Vector3 relativePosition = vertex - firstHitPoint;
        // Debug.Log("relativePosition:" + relativePosition + "line direction:" + lineDirection);
        // Check if the vertex is above the line using the dot product
        Vector3 crossProduct = Vector3.Cross(relativePosition, lineDirection);
        bool isAbove = Vector3.Dot(crossProduct, Vector3.forward) > 0;
        Debug.Log("Checking is vertex above? vertex:" + vertex + "first hit point" + firstHitPoint + "relative direction:" + relativePosition
        + "line direction" + lineDirection + "is above?" + isAbove);
        return isAbove;
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
    void CreateMesh(string name, List<int> triangles, List<Vector3> vertices, bool isFirstMesh)
    {
        //debug print triangles and vertices
        Debug.Log("debugging mesh vertices for :" + name);
        for (int i = 0; i < triangles.Count; i += 3)
        {
            
            
            // Get the vertices of the current triangle
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];

            Debug.Log("triangle:" + triangles[i] + "vertices" + v1);
            Debug.Log("triangle:" + triangles[i+1] + "vertices" + v2);
            Debug.Log("triangle:" + triangles[i+2] + "vertices" + v3);
        }

        if (isFirstMesh){
            firstSlicedPlane.gameObject.SetActive(true);
            firstSlicedPlane.mesh.vertices = vertices.ToArray();
            firstSlicedPlane.mesh.triangles = triangles.ToArray();
            // Recalculate normals for correct lighting
            firstSlicedPlane.mesh.RecalculateNormals();
        }
        else{
            secondSlicedPlane.gameObject.SetActive(true);
            secondSlicedPlane.mesh.vertices = vertices.ToArray();
            secondSlicedPlane.mesh.triangles = triangles.ToArray();
            // Recalculate normals for correct lighting
            secondSlicedPlane.mesh.RecalculateNormals();
        }
        plane.SetActive(false);
    }

    void MoveCamera(){
        cameraAnimator.SetBool("StartMoving", true);
    }
}
