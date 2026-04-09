using UnityEngine;

[CreateAssetMenu(menuName = "World/BiomeGenerator")]
public class BiomeGenerator : ScriptableObject {

    [Header("Noise Settings")]
    public float heightScale = 0.05f;
    public float biomeScale = 0.01f;
    public int seed = 12345;
    public int baseHeight = 40;
    public int heightVariation = 20;

    public int GetHeight(int x, int z) {
        float h = Mathf.PerlinNoise((x + seed) * heightScale, (z + seed) * heightScale);
        return baseHeight + Mathf.RoundToInt(h * heightVariation);
    }

    public BlockType GetSurfaceBlock(int x, int z) {
        float biomeVal = Mathf.PerlinNoise((x + seed) * biomeScale, (z + seed) * biomeScale);

        if (biomeVal < 0.33f)
            return BlockType.Sand;     // Desert
        else if (biomeVal < 0.66f)
            return BlockType.Grass;    // Plains
        else
            return BlockType.Grass;    // Forest (trees can be added later)
    }
}
