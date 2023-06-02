using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI.Table;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
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

        Vector2 center = Vector2.one * resolution / 2;

        for (int z = 0; z < resolution + 1; z++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                Vector2 curr = new Vector2(x, z);
                float factor = Mathf.SmoothStep(1f, 0f, 5 - (curr - center).magnitude);

                float y = factor * Mathf.PerlinNoise(x * .3f, z * .3f) * roughness;
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
        Transform objectParent = transform.Find("Objects");
        Vector2 center = Vector2.one * resolution / 2;

        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                // Place rock
                Vector2 curr = new Vector2(row, column);
                if ((curr - center).magnitude < 3)
                    continue;

                int rand = Random.Range(0, 11);
                if (rand == 0)
                {
                    int i = (row * resolution) + row + column;
                    Instantiate(rockPrefabs[i % rockPrefabs.Length], vertices[i] - (Vector3.up * 0.25f), Quaternion.Euler(0f, Random.Range(0, 360), 0f), objectParent);
                }
                else if (rand >= 9)
                {
                    // Place plant
                    int i = (row * resolution) + row + column;
                    Instantiate(plantPrefabs[i % plantPrefabs.Length], vertices[i], Quaternion.Euler(0f, Random.Range(0, 360), 0f), objectParent);
                }
            }
        }
    }

    float map(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}
