using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    private Player player_;
    private FloorChunk floor_chunk_;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player_ = FindAnyObjectByType<Player>();
        floor_chunk_ = FindAnyObjectByType<FloorChunk>();
    }

    void Update()
    {
        CoinManager.instance.UpdateCoins();
        CoinManager.instance.UpdateTextOnScreen();
    }

    private void FixedUpdate()
    {
        player_.PlayerControl();
        player_.UpdateCollisionBox();

        floor_chunk_.UpdateChunks();
        if (player_.is_dashing)
        {
            floor_chunk_.UpdateChunkSpeed();
        } else
        {
            floor_chunk_.speed = 2.0f;
        }
    }
}
