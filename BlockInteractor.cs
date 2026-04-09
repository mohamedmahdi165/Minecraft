using UnityEngine;

public class BlockInteractor : MonoBehaviour {

    public float reach = 6f;
    public WorldManager world;
    public BlockType placeBlockType = BlockType.Dirt;

    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            // Destroy block
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, reach)) {
                Vector3 targetPos = hit.point - hit.normal * 0.01f;

                if (world.TryGetChunk(targetPos, out var chunk, out var localPos)) {
                    chunk.SetBlock(localPos.x, localPos.y, localPos.z, BlockType.Air);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            // Place block
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, reach)) {
                Vector3 targetPos = hit.point + hit.normal * 0.01f;

                if (world.TryGetChunk(targetPos, out var chunk, out var localPos)) {
                    chunk.SetBlock(localPos.x, localPos.y, localPos.z, placeBlockType);
                }
            }
        }
    }
}
