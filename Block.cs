using UnityEngine;

public enum BlockType {
    Air,
    Grass,
    Dirt,
    Stone,
    Sand,
    Water
}

[System.Serializable]
public struct Block {
    public BlockType type;

    public bool IsSolid => type != BlockType.Air && type != BlockType.Water;

    public Block(BlockType type) {
        this.type = type;
    }
}
