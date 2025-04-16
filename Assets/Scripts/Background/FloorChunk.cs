using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

public class FloorChunk : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject floor_prefab_;
    [SerializeField] private GameObject chunk_prefab_;
    [SerializeField] private GameObject[] possible_coins_;
    [SerializeField] private GameObject[] possible_enemies_;
    [SerializeField] private GameObject[] possible_obstacles_;
    [SerializeField] private GameObject portal_;

    [Header("Chunk Settings")]
    [SerializeField] private int tiles_per_chunk_ = 5;
    [SerializeField] private float tile_width_ = 3.5f;
    [SerializeField] private float chunk_width_ = 17.5f;

    [SerializeField] private int chunk_counter;
    [SerializeField] private int coin_spawn_chance_;
    [SerializeField] private int enemy_spawn_chance_;
    [SerializeField] private int ob_spawn_chance_;
    [SerializeField] private int portal_spawn_chance_;

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

    public bool type_spawn;

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
        GameObject chunk = new GameObject("FloorClearChunk");

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
                                          "Assets/Entities/Floor/FloorClearChunk.prefab");
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

    public void ResetSpeed()
    {
        speed = 2.0f;
    }

    public void IncrementSpeed(float increment)
    {
        speed += increment;
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
        if (active_chunks_.Count > 0)
        {
            PopulateChunk(new_chunk);
            PopulateObstacles(new_chunk);
        }
        active_chunks_.Add(new_chunk);
    }

    void PopulateChunk(GameObject chunk)
    {
        ChunkData data = chunk.GetComponent<ChunkData>();
        if (data == null) return;

        if (possible_coins_.Length != 0)
        {
            foreach (Transform coin_point in data.coin_spawn_points)
            {
                int roll = Random.Range(0, 100);
                if (roll < coin_spawn_chance_ && possible_coins_.Length > 0)
                {
                    int coin_index = Random.Range(0, possible_coins_.Length);
                    Instantiate(possible_coins_[coin_index],
                                coin_point.position,
                                Quaternion.identity,
                                chunk.transform);
                }
            }
        }

        if (possible_enemies_.Length != 0)
        {
            foreach (Transform enemy_point in data.enemy_spawn_points)
            {
                int roll = Random.Range(0, 100);
                if (roll < enemy_spawn_chance_ && possible_enemies_.Length > 0)
                {
                    int enemy_index = Random.Range(0, possible_enemies_.Length);
                    GameObject prefab = possible_enemies_[enemy_index];
                    Instantiate(prefab,
                                enemy_point.position,
                                prefab.transform.rotation,
                                chunk.transform);
                }
            }
        }

        if (portal_ != null)
        {
            foreach (Transform portal_point in data.portal_spawn_points)
            {
                int roll = Random.Range(0, 100);
                if (roll < portal_spawn_chance_)
                {

                    GameObject porta_prefab = portal_;
                    Instantiate(porta_prefab,
                                portal_point.position,
                                porta_prefab.transform.rotation,
                                chunk.transform);
                }
            }
        }
    }

    void PopulateObstacles(GameObject chunk)
    {
        if (possible_obstacles_.Length != 0)
        {
            float spawn_y = chunk.transform.position.y + 1.5f;

            for (int i = 0; i < tiles_per_chunk_; i++)
            {
                float x_offset = (i * tile_width_) - (chunk_width_ / 2.0f);
                Vector3 spawn_point = new Vector3(chunk.transform.position.x + x_offset, spawn_y, 0f);

                int roll = Random.Range(0, 100);
                if (roll < ob_spawn_chance_)
                {
                    int enemy_index = Random.Range(0, possible_obstacles_.Length);
                    GameObject prefab = possible_obstacles_[enemy_index];
                    Instantiate(prefab, spawn_point, Quaternion.identity, chunk.transform);
                }
            }
        }
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
