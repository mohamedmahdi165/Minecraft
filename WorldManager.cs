using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public Transform player;
    public GameObject chunkPrefab;
    public BiomeGenerator biomeGenerator;
    public int viewDistanceInChunks = 4;

    private Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    private Vector2Int currentPlayerChunk;

    void Start() {
        UpdateWorld(true);
    }

    void Update() {
        UpdateWorld(false);
    }

    void UpdateWorld(bool force) {
        Vector2Int playerChunk = WorldToChunkCoord(player.position);

        if (!force && playerChunk == currentPlayerChunk)
            return;

        currentPlayerChunk = playerChunk;

        HashSet<Vector2Int> needed = new HashSet<Vector2Int>();

        for (int x = -viewDistanceInChunks; x <= viewDistanceInChunks; x++) {
            for (int z = -viewDistanceInChunks; z <= viewDistanceInChunks; z++) {
                needed.Add(new Vector2Int(playerChunk.x + x, playerChunk.y + z));
            }
        }

        // Unload chunks
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in chunks) {
            if (!needed.Contains(kvp.Key)) {
                Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var c in toRemove)
            chunks.Remove(c);

        // Load new chunks
        foreach (var coord in needed) {
            if (!chunks.ContainsKey(coord))
                CreateChunk(coord);
        }
    }

    void CreateChunk(Vector2Int coord) {
        Vector3 worldPos = new Vector3(
            coord.x * Chunk.ChunkSize,
            0,
            coord.y * Chunk.ChunkSize
        );

        GameObject go = Instantiate(chunkPrefab, worldPos, Quaternion.identity, transform);
        Chunk chunk = go.GetComponent<Chunk>();
        chunk.Initialize(coord);

        int worldOffsetX = coord.x * Chunk.ChunkSize;
        int worldOffsetZ = coord.y * Chunk.ChunkSize;

        chunk.Generate(biomeGenerator, worldOffsetX, worldOffsetZ);

        chunks.Add(coord, chunk);
    }

    public Vector2Int WorldToChunkCoord(Vector3 pos) {
        int cx = Mathf.FloorToInt(pos.x / Chunk.ChunkSize);
        int cz = Mathf.FloorToInt(pos.z / Chunk.ChunkSize);
        return new Vector2Int(cx, cz);
    }

    public bool TryGetChunk(Vector3 worldPos, out Chunk chunk, out Vector3Int localPos) {
        int cx = Mathf.FloorToInt(worldPos.x / Chunk.ChunkSize);
        int cz = Mathf.FloorToInt(worldPos.z / Chunk.ChunkSize);

        Vector2Int coord = new Vector2Int(cx, cz);

        if (chunks.TryGetValue(coord, out chunk)) {
            int lx = Mathf.FloorToInt(worldPos.x - coord.x * Chunk.ChunkSize);
            int ly = Mathf.FloorToInt(worldPos.y);
            int lz = Mathf.FloorToInt(worldPos.z - coord.y * Chunk.ChunkSize);

            localPos = new Vector3Int(lx, ly, lz);
            return true;
        }

        localPos = Vector3Int.zero;
        return false;
    }
}
