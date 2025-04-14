using Mono.Cecil.Cil;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [Header("Stats")]
    public double worth;

    [Header("OnDestroy")]
    [SerializeField] private GameObject on_death_;
    [SerializeField] private AudioClip pick_sfx_;

    [Header("Handlers")]
    private BoxCollider2D box_collider_;
    private SpriteRenderer sprite_renderer_;

    private void Start()
    {
        box_collider_ = GetComponent<BoxCollider2D>();
        sprite_renderer_ = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateCollisionBox();
    }

    private void OnDestroy()
    {
        Instantiate(on_death_, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(pick_sfx_, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
