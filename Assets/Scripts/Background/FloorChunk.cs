using UnityEngine;

public class FloorChunk : MonoBehaviour
{
    [SerializeField] private GameObject floor_prefab_;

    [SerializeField] private int tiles_per_chunk_ = 5;

    [SerializeField] private float tile_width = 3.5f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateChunkPrefab()
    {
        GameObject chunk = new GameObject("FloorChunk");

        for (int i = 0; i < tiles_per_chunk_; i++)
        {
            GameObject tile = Instantiate(floor_prefab_,
                                          new Vector3(i * tile_width, 0, 0),
                                          Quaternion.identity,
                                          chunk.transform);
            tile.name = $"FloorTile_{i}";
        }
        #if UNITY_EDITOR
                UnityEditor.PrefabUtility.SaveAsPrefabAsset(chunk, 
                                          "Assets/Entities/Floor/FloorChunk.prefab");
        #endif

        DestroyImmediate(chunk);
    }
}
