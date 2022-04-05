using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class DynamicPlaneBuilder : MonoBehaviour
{
    public bool EnableDebug = true;
    private int _sizeX;
    private int _sizeZ;
    [Range(1, 10)]
    public int LOD = 5;
    public int SizeX = 10;
    public int SizeZ = 10;
    [Range(0.1f, 2.0f)]
    public float UpdateDelay = 0.3f;

    Mesh mesh;

    List<Vector3> verts = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triangles = new List<int>();

    BoxCollider boxCollider;
    float deltaTime = 0;

    void Start()
    {   
        this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
        this.GenerateMesh();
    }

    private void GenerateMesh()
    {

        this.mesh = this.gameObject.GetComponent<MeshFilter>().mesh;
        
        //Clear everything
        this.verts.Clear();
        this.normals.Clear();
        this.uvs.Clear();
        this.mesh.Clear();

        this.RecalculateWithLOD();
        this.mesh.MarkDynamic();

        this.GenerateVerts();
        this.BuildTriangles();

        this.mesh.vertices = this.verts.ToArray();
        this.mesh.uv = this.uvs.ToArray();
        this.mesh.triangles = this.triangles.ToArray();
        this.mesh.normals = this.normals.ToArray();
        this.mesh.Optimize();

        this.boxCollider.size = this.mesh.bounds.size;
        this.boxCollider.center = this.mesh.bounds.size / 2;
    }

    private void GenerateVerts()
    {
        /*

        Example: 2x2 Plane

        Verticies:
          0    1    2
        {0,0}{1,0}{2,0}
          3    4    5
        {0,1}{1,1}{2,1}
          6    7    8
        {0,2}{1,2}{2,2}

        Triangles:
        0,3,1   3,1,4   1,2,4   4,2,5
        3,4,6   6,4,7   4,5,7   7,5,8

        */

        for (float Z = 0; Z <= this._sizeZ; Z++)
        {
            for (float X = 0; X <= this._sizeX; X++)
            {
                Vector3 vert = new Vector3(X / this.LOD, 0, Z / this.LOD);
                this.verts.Add(vert);

                //TODO: Being lazy here. Need to handle Normals properly at some stage.
                this.normals.Add(Vector3.up);
                this.uvs.Add(new Vector2(X / (float)_sizeX, Z / (float)_sizeZ));
            }
        }
    }

    void BuildTriangles()
    {
        this.triangles.Clear();

        int v1 = 0;
        int v2 = 0;
        int v3 = 0;

        for (int z = 0; z < this._sizeZ; z++)
        {
            int row_key = (this._sizeX + 1) * z;

            for (int x = 0; x < this._sizeX; x++)
            {
                //even triangle
                v1 = x + row_key;
                v2 = v1 + 1;
                v3 = v2 + this._sizeX;

                this.triangles.Add(v1);
                this.triangles.Add(v2);
                this.triangles.Add(v3);

                //odd triangle
                v1 = v3;
                v3 = v1 + 1;

                this.triangles.Add(v1);
                this.triangles.Add(v2);
                this.triangles.Add(v3);
            }
        }

        triangles.Reverse();
    }

    private void RecalculateWithLOD()
    {
        this._sizeX = this.SizeX * this.LOD;
        this._sizeZ = this.SizeZ * this.LOD;
    }

    void Update()
    {
        if (this.deltaTime >= 1f)
        {
            this.GenerateMesh();
            
            this.deltaTime = 0;
        }
        else
            this.deltaTime += Time.deltaTime;

        if (this.EnableDebug)
            for (int i = 0; i < triangles.Count-1; i++)
            {
                
                Debug.DrawLine(verts[triangles[i]]+this.transform.position, verts[triangles[i+1]]+this.transform.position, Color.magenta);
            }

    }
}
