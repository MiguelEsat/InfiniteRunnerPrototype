using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public double worth;

    [Header("Handlers")]
    private BoxCollider2D box_collider_;
    private SpriteRenderer sprite_renderer_;
    

    void Start()
    {
        box_collider_ = GetComponent<BoxCollider2D>();
        sprite_renderer_ = GetComponent<SpriteRenderer>();
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

    public void UpdateCollisionBox()
    {
        if (sprite_renderer_.sprite != null)
        {
            box_collider_.offset = sprite_renderer_.sprite.bounds.center;
            box_collider_.size = sprite_renderer_.sprite.bounds.size;
        }
    }
}
