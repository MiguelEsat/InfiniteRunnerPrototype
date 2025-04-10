using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

public class FloorChunk : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject floor_prefab_;
    [SerializeField] private GameObject chunk_prefab_;

    [Header("Chunk Settings")]
    [SerializeField] private int tiles_per_chunk_ = 5;
    [SerializeField] private float tile_width_ = 3.5f;
    [SerializeField] private float chunk_width_ = 17.5f;
    [SerializeField] private int chunk_counter;

    [Header("Chunk Pooling")]
    [SerializeField] private int preload_count = 3;
    [SerializeField] private Transform chunk_parent_;

    private float camera_right_edge_;
    private float camera_left_edge_;
    private float next_spawn_x_;
    private float camera_half_width_;

    [Header("Chunk Obstacles")]

    private List<GameObject> active_chunks_ = new List<GameObject>();
    private Queue<GameObject> pooled_chunk_ = new Queue<GameObject>();
    
    private Rigidbody2D rigidbody_;
    private Camera cam;

    [Header("Scrolling")]
    public float speed;

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        camera_right_edge_ = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x;
        camera_half_width_ = cam.orthographicSize * cam.aspect;

        chunk_parent_ = transform;

        InitializePool();
        SpawnInitialChunks();

        speed = 2.0f;
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
                                          new Vector3(i * tile_width_, 0, 0),
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

    private void AlignTiles()
    {
        foreach (Transform tile in transform)
        {
            tile.position = new Vector3(Mathf.Round(
                                        tile.position.x / tile_width_) * tile_width_,
                                        0.0f,
                                        0.0f);
        }
    }

    public void UpdateChunkSpeed()
    {
        speed = 6.0f;
    }

    public void UpdateChunks()
    {
        rigidbody_.linearVelocity = new Vector2(-speed, 
                                                rigidbody_.linearVelocity.y);

        camera_right_edge_ = cam.transform.position.x + camera_half_width_;
        camera_left_edge_ = cam.transform.position.x - camera_half_width_;

        float last_chunk_right_edge = GetChunkRightEdge(active_chunks_[active_chunks_.Count - 1]);

        if (camera_right_edge_ > last_chunk_right_edge - 1)
        {
            SpawnChunk();
        }

        if (active_chunks_.Count > 0)
        {
            float chunk_right_edge = GetChunkRightEdge(active_chunks_[0]);
            if ((chunk_right_edge + 2.0f) < (camera_left_edge_ - 5.0f))
            {
                DeleteOldestChunk();
            }
        }
    }

    #region [Chunk Pooling]

    private void InitializePool()
    {
        for (int i = 0; i < preload_count + 2; i++)
        {
            GameObject chunk = Instantiate(chunk_prefab_,
                                           Vector3.zero, 
                                           Quaternion.identity, 
                                           chunk_parent_); 
            chunk.SetActive(false);
            pooled_chunk_.Enqueue(chunk);
        }
    }

    private GameObject GetPooledChunk()
    {
        if (pooled_chunk_.Count > 0)
        {
            GameObject chunk = pooled_chunk_.Dequeue();
            chunk.SetActive(true);
            return chunk;
        }

        GameObject new_chunk = Instantiate(chunk_prefab_,
                                           chunk_parent_);
        return new_chunk;
    }

    private void ReturnToPool(GameObject chunk)
    {
        chunk.SetActive(false);
        pooled_chunk_.Enqueue(chunk);
    }

    #endregion

    #region [Chunk Spawning]

    private void SpawnInitialChunks()
    {
        for (int i = 0; i < preload_count; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        GameObject new_chunk = GetPooledChunk();
        float spawn_x = 0.0f;

        if (active_chunks_.Count > 0)
        {
            GameObject last_chunk = active_chunks_[active_chunks_.Count - 1];
            float last_chunk_right_edge = GetChunkRightEdge(last_chunk);
            spawn_x = last_chunk_right_edge + (chunk_width_ * 0.5f);
        }
        else
        {
            spawn_x = cam.transform.position.x - camera_half_width_ + (chunk_width_ * 0.10f);
        }

        new_chunk.transform.position = new Vector3(spawn_x, -4.0f, 0.0f);
        active_chunks_.Add(new_chunk);
    }

    private void DeleteOldestChunk()
    {
        GameObject oldest_chunk = active_chunks_[0];
        active_chunks_.RemoveAt(0);
        ReturnToPool(oldest_chunk);
    }

    private float GetChunkLeftEdge(GameObject chunk)
    {
        return chunk.transform.position.x - (chunk_width_ * 0.5f);
    }

    private float GetChunkRightEdge(GameObject chunk)
    {
        return chunk.transform.position.x + (chunk_width_ * 0.5f);
    }

    #endregion

    void OnDrawGizmosSelected()
    {
   
        if (cam == null) return;

        Gizmos.color = Color.green;
        float cam_height = cam.orthographicSize * 2;
        Vector3 cam_pos = cam.transform.position;

        Gizmos.DrawWireCube(
            new Vector3(cam_pos.x, cam_pos.y, 0),
            new Vector3(camera_half_width_ * 2, cam_height, 0)
        );

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(next_spawn_x_, cam_pos.y - 5, 0),
            new Vector3(next_spawn_x_, cam_pos.y + 5, 0)
        );
    }
}
