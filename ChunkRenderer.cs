using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour {
    public bool Dirty { get; set; } = true;

    private MeshFilter meshFilter;

    void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    void LateUpdate() {
        if (Dirty) {
            RebuildMesh();
            Dirty = false;
        }
    }

    void RebuildMesh() {
        Chunk chunk = GetComponent<Chunk>();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < Chunk.ChunkSize; x++) {
            for (int y = 0; y < Chunk.ChunkHeight; y++) {
                for (int z = 0; z < Chunk.ChunkSize; z++) {

                    Block block = chunk.blocks[x, y, z];
                    if (!block.IsSolid) continue;

                    Vector3 pos = new Vector3(x, y, z);

                    AddFace(chunk, block.type, pos, x + 1, y, z, Vector3.right, vertices, triangles, uvs);
                    AddFace(chunk, block.type, pos, x - 1, y, z, Vector3.left, vertices, triangles, uvs);
                    AddFace(chunk, block.type, pos, x, y + 1, z, Vector3.up, vertices, triangles, uvs);
                    AddFace(chunk, block.type, pos, x, y - 1, z, Vector3.down, vertices, triangles, uvs);
                    AddFace(chunk, block.type, pos, x, y, z + 1, Vector3.forward, vertices, triangles, uvs);
                    AddFace(chunk, block.type, pos, x, y, z - 1, Vector3.back, vertices, triangles, uvs);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    void AddFace(
        Chunk chunk,
        BlockType type,
        Vector3 blockPos,
        int nx, int ny, int nz,
        Vector3 normal,
        List<Vector3> vertices,
        List<int> triangles,
        List<Vector2> uvs
    ) {
        bool visible = false;

        if (nx < 0 || nx >= Chunk.ChunkSize ||
            ny < 0 || ny >= Chunk.ChunkHeight ||
            nz < 0 || nz >= Chunk.ChunkSize) {
            visible = true;
        } else {
            if (!chunk.blocks[nx, ny, nz].IsSolid)
                visible = true;
        }

        if (!visible) return;

        int vStart = vertices.Count;

        Vector3[] faceVerts = VoxelMeshData.GetFaceVertices(normal);
        foreach (var v in faceVerts)
            vertices.Add(blockPos + v);

        triangles.Add(vStart + 0);
        triangles.Add(vStart + 1);
        triangles.Add(vStart + 2);
        triangles.Add(vStart + 2);
        triangles.Add(vStart + 3);
        triangles.Add(vStart + 0);

        Vector2[] faceUVs = TextureAtlas.GetUVs(type);
        uvs.AddRange(faceUVs);
    }
}
