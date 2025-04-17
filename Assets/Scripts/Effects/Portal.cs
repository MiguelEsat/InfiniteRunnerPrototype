using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private AudioClip col_sfx_;

    void Start()
    {
            
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(col_sfx_, transform.position);
        }
    }
}
