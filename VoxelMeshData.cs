using UnityEngine;

public static class VoxelMeshData {

    // Returns the 4 vertices for a face based on its normal direction
    public static Vector3[] GetFaceVertices(Vector3 normal) {

        if (normal == Vector3.up) {
            return new Vector3[] {
                new Vector3(0,1,0),
                new Vector3(1,1,0),
                new Vector3(1,1,1),
                new Vector3(0,1,1)
            };
        }

        if (normal == Vector3.down) {
            return new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(0,0,1),
                new Vector3(1,0,1),
                new Vector3(1,0,0)
            };
        }

        if (normal == Vector3.forward) {
            return new Vector3[] {
                new Vector3(0,0,1),
                new Vector3(0,1,1),
                new Vector3(1,1,1),
                new Vector3(1,0,1)
            };
        }

        if (normal == Vector3.back) {
            return new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(1,0,0),
                new Vector3(1,1,0),
                new Vector3(0,1,0)
            };
        }

        if (normal == Vector3.right) {
            return new Vector3[] {
                new Vector3(1,0,0),
                new Vector3(1,0,1),
                new Vector3(1,1,1),
                new Vector3(1,1,0)
            };
        }

        // Left face
        return new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(0,0,1)
        };
    }
}
