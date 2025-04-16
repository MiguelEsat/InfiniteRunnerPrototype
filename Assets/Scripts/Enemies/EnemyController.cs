using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public double worth;

    [Header("Handlers")]
    private BoxCollider2D box_collider_;
    private SpriteRenderer sprite_renderer_;

    private bool is_updated = false;

    [SerializeField] private GameObject death_effect_;
    [SerializeField] private AudioClip death_sfx_;

    private bool on_application_quit = false;

    void Start()
    {
        box_collider_ = GetComponent<BoxCollider2D>();
        sprite_renderer_ = GetComponent<SpriteRenderer>();
        on_application_quit = false;
    }

    
    void Update()
    {
        UpdateCollisionBox();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoinManager.instance.EarnCoins((float)worth);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoinManager.instance.EarnCoins((float)worth);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying || on_application_quit) return;

        Instantiate(death_effect_, 
                    transform.position, 
                    Quaternion.identity);
        AudioSource.PlayClipAtPoint(death_sfx_, transform.position);
    }

    private void OnApplicationQuit()
    {
        on_application_quit = true;
    }

    public void UpdateCollisionBox()
    {
        if (sprite_renderer_.sprite != null && !is_updated)
        {
            box_collider_.offset = sprite_renderer_.sprite.bounds.center;
            box_collider_.size = sprite_renderer_.sprite.bounds.size;

            is_updated = true;
        }
    }
}
