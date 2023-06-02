using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class EnvironmentGenerator : MonoBehaviour
{
    public Vector2 size;
    public int resolution;
    public float roughness;

    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private GameObject[] plantPrefabs;

    private Mesh groundMesh;

    private List<Vector3> vertices;
    private List<int> triangles;

    private void Awake()
    {
        groundMesh = new();
        GetComponent<MeshFilter>().mesh = groundMesh;
    }
    void Start()
    {
        GeneratePlane(size, resolution, roughness);
        AssignMesh();
        gameObject.AddComponent<MeshCollider>();

        Populate();
    }

    private void GeneratePlane(Vector2 size, int resolution, float roughness)
    {
        vertices = new();
        triangles = new();

        float xPerStep = size.x / resolution;
        float zPerStep = size.y / resolution;
        for (int z = 0; z < resolution + 1; z++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * roughness;
                vertices.Add(new Vector3((x * xPerStep) - ((size.x) / 2f), y, (z * zPerStep) - ((size.y) / 2f)));
            }
        }

        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = (row * resolution) + row + column;

                // first triangle
                triangles.Add(i);
                triangles.Add(i + (resolution) + 1);
                triangles.Add(i + (resolution) + 2);

                // secondtriangle
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }
    }

    private void AssignMesh()
    {
        groundMesh.Clear();
        groundMesh.vertices = vertices.ToArray();
        groundMesh.triangles = triangles.ToArray();
    }

    private void Populate()
    {

    }

}
