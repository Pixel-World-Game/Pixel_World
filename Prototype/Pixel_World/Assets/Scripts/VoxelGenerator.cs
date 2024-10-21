using UnityEngine;

public class VoxelGenerator : MonoBehaviour
{
    public int width = 16;
    public int height = 16;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                }
            }
        }
    }
}

