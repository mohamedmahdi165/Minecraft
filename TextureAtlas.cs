using UnityEngine;

public static class TextureAtlas {
    // Number of tiles per row/column in the atlas (e.g., 4x4)
    private const int atlasSize = 4;
    private static readonly float tileSize = 1f / atlasSize;

    // Returns the UV coordinates for a block face
    public static Vector2[] GetUVs(BlockType type) {
        Vector2 offset = GetOffset(type);

        return new Vector2[] {
            offset + new Vector2(0, 0),
            offset + new Vector2(tileSize, 0),
            offset + new Vector2(tileSize, tileSize),
            offset + new Vector2(0, tileSize)
        };
    }

    // Determines which tile in the atlas corresponds to the block type
    private static Vector2 GetOffset(BlockType type) {
        int index = type switch {
            BlockType.Grass => 0,
            BlockType.Dirt  => 1,
            BlockType.Stone => 2,
            BlockType.Sand  => 3,
            BlockType.Water => 4,
            _ => 15 // fallback tile
        };

        int x = index % atlasSize;
        int y = index / atlasSize;

        return new Vector2(x * tileSize, y * tileSize);
    }
}
