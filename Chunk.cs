using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    public const int ChunkSize = 16;
    public const int ChunkHeight = 128;

    public Vector2Int coord;
    public Block[,,] blocks;

    private ChunkRenderer rendererComponent;

    void Awake() {
        rendererComponent = GetComponent<ChunkRenderer>();
        blocks = new Block[ChunkSize, ChunkHeight, ChunkSize];
    }

    public void Initialize(Vector2Int coord) {
        this.coord = coord;
        name = $"Chunk_{coord.x}_{coord.y}";
    }

    public Block GetBlock(int x, int y, int z) {
        if (!InBounds(x, y, z))
            return new Block(BlockType.Air);

        return blocks[x, y, z];
    }

    public void SetBlock(int x, int y, int z, BlockType type) {
        if (!InBounds(x, y, z))
            return;

        blocks[x, y, z] = new Block(type);
        rendererComponent.Dirty = true;
    }

    private bool InBounds(int x, int y, int z) {
        return x >= 0 && x < ChunkSize &&
               y >= 0 && y < ChunkHeight &&
               z >= 0 && z < ChunkSize;
    }

    public void Generate(BiomeGenerator biomeGen, int worldOffsetX, int worldOffsetZ) {
        for (int x = 0; x < ChunkSize; x++) {
            for (int z = 0; z < ChunkSize; z++) {

                int wx = worldOffsetX + x;
                int wz = worldOffsetZ + z;

                int height = biomeGen.GetHeight(wx, wz);
                BlockType surface = biomeGen.GetSurfaceBlock(wx, wz);

                for (int y = 0; y < ChunkHeight; y++) {

                    if (y > height) {
                        blocks[x, y, z] = new Block(BlockType.Air);
                    }
                    else if (y == height) {
                        blocks[x, y, z] = new Block(surface);
                    }
                    else if (y > height - 3) {
                        blocks[x, y, z] = new Block(BlockType.Dirt);
                    }
                    else {
                        blocks[x, y, z] = new Block(BlockType.Stone);
                    }
                }
            }
        }

        rendererComponent.Dirty = true;
    }
}

