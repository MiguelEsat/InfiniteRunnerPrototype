using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

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
        
    }

    private void FixedUpdate()
    {
        player_.PlayerControl();
    }
}
