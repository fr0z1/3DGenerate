using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Noise : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private float seed;

    [SerializeField] private List<Vector3> vertices;
    [SerializeField] private List<int> tris;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Mesh mesh;
    private void Start()
    {
        mesh = new Mesh();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        seed = Random.Range(0f, 1000000f);

        GenerateVertices();
        //GenerateTris();
        StartCoroutine(GenerateTrisCor());

        mesh.name = "Place";
        meshFilter.mesh = mesh;
        mesh.triangles = tris.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void GenerateVertices()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                var y = Mathf.PerlinNoise((x + seed) * 0.05f, (z + seed) * 0.05f) * 5f;
                vertices.Add(new Vector3(x*0.5f, y, z*0.5f));
            }
        }

        mesh.vertices = vertices.ToArray<Vector3>();
    }

    private void GenerateTris()
    {
        for (int x = 0; x < size.x - 1; x++)
        {
            for (int y = 0; y < size.y - 1; y++)
            {
                tris.Add(size.y * x + y);
                tris.Add(size.y * x + y + 1);
                tris.Add(size.y * x + y + size.y);

                tris.Add(size.y * x + y + size.y + 1);
                tris.Add(size.y * x + y + size.y );
                tris.Add(size.y * x + y + 1);
            }
        }
       
    }
    IEnumerator GenerateTrisCor()
    {
        for (int x = 0; x < size.x - 1; x++)
        {
            for (int y = 0; y < size.y - 1; y++)
            {
                tris.Add(size.y * x + y);
                tris.Add(size.y * x + y + 1);
                tris.Add(size.y * x + y + size.y);

                tris.Add(size.y * x + y + size.y + 1);
                tris.Add(size.y * x + y + size.y);
                tris.Add(size.y * x + y + 1);

                yield return new WaitForSeconds(0.01f);

                mesh.triangles = tris.ToArray();

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
            }
        }
    }
}