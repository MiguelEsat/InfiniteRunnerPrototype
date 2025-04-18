using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Animator animator_;
    private BoxCollider2D box_collider_;
    private Rigidbody2D rigidbody_;
    private SpriteRenderer sprite_;

    [Header("Jump Physics")]
    [SerializeField] private float jump_force_ = 12.0f;
    [SerializeField] private float dash_jump_force_ = 8.0f;
    [SerializeField] private float horizontal_drift_ = 1.0f;
    [SerializeField] private float dash_horizontal_drift_ = 4.0f;

    [Header("Gravity Control")]
    [SerializeField] private float base_gravity_ = 2.0f;
    [SerializeField] private float dash_gravity_multiplier_ = 0.5f;
    [SerializeField] private float ground_check_distance_ = 3.0f;

    [Header("Other Variables")]
    private float dash_timer_;
    public float dash_time;
    public float dash_cd;
    private float cooldown_timer_ = 0.0f;

    [SerializeField] private float jump_hold_time = 0.1f;
    private float jump_hold_timer = 0.0f;

    private bool is_updated = false;

    public bool is_grounded = false;
    public bool is_dashing = false;
    public bool is_on_cooldown = false;
    public bool is_dead = false;

    [SerializeField] private LayerMask ground_layer_;
    public Transform ground_check;



    void Start()
    {
        animator_ = GetComponent<Animator>();   
        box_collider_ = GetComponent<BoxCollider2D>();
        rigidbody_ = GetComponent<Rigidbody2D>();
        sprite_ = GetComponent <SpriteRenderer>();

        dash_cd = 10.0f;
    }

    void FixedUpdate()
    {
        is_grounded = Physics2D.OverlapCapsule(ground_check.position, new Vector2(1.8f, 0.3f),
                                       CapsuleDirection2D.Horizontal, 0, ground_layer_);
        if(transform.position.y < -5.0f)
        {
            GameManager.instance.start_timer = true;
            animator_.SetBool("IsDead", true);
        }
        UpdateCollisionBox();
    }

    public void UpdateCollisionBox()
    {
        if (sprite_.sprite != null && !is_updated)
        {
            box_collider_.offset = sprite_.sprite.bounds.center;
            box_collider_.size = sprite_.sprite.bounds.size;

            is_updated = true;  
        }
    }

    public void PlayerControl()
    {
        if (!is_dashing)
        {
            StartRunnig();
        }
        Jump();
        DashingTimer();
    }

    private void StartRunnig()
    {
        rigidbody_.gravityScale = is_dashing ? base_gravity_ * dash_gravity_multiplier_ 
                                               : base_gravity_;   
        animator_.SetFloat("Velocity", 1.0f);   
    }

    public void StartDashing()
    {
        if (!is_on_cooldown)
        {
            is_dashing = true;
            animator_.SetFloat("Velocity", 2.0f);
        }
    }

    private void DashingTimer()
    {
        if (is_dashing)
        {
            dash_timer_ += Time.deltaTime;
            if (dash_timer_ >= dash_time)
            {
                is_dashing = false;
                is_on_cooldown = true;
                dash_timer_ -= dash_timer_;
            }
        } else if (is_on_cooldown)
        {
            cooldown_timer_ += Time.deltaTime;

            if (cooldown_timer_ >= dash_cd)
            {
                is_on_cooldown = false;
                cooldown_timer_ -= cooldown_timer_;
            }

        }
    }

    private void Jump()
    {
            animator_.SetFloat("VerticalVel", rigidbody_.linearVelocity.y);
            animator_.SetBool("IsJumping", false);
        
            if (Input.GetButtonDown("Jump") && is_grounded)
            {
                float current_hori_drift = is_dashing ? dash_horizontal_drift_ : horizontal_drift_;
                float current_jump_force = is_dashing ? dash_jump_force_ : jump_force_;
                rigidbody_.linearVelocity = new Vector2(rigidbody_.linearVelocityX, current_jump_force);

                animator_.SetFloat("VerticalVel", rigidbody_.linearVelocity.y); 
            }

            if (!is_grounded && Input.GetKey(KeyCode.DownArrow))
            {
                float fall_speed_multiplier = 2.0f;
                rigidbody_.linearVelocity = new Vector2(
                    rigidbody_.linearVelocityX,
                    rigidbody_.linearVelocity.y - fall_speed_multiplier * Time.deltaTime * 20f
                );
            }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            animator_.SetTrigger("Attack");
        }
        if (collision.gameObject.CompareTag("Portal"))
        {
            GameManager.instance.is_scene_changing = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex 
            + GameManager.instance.RandomNumber());
        }
        if (GameManager.instance.mini_game_timer > 0 && !GameManager.instance.start_timer)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                GameManager.instance.start_timer = true;
                animator_.SetBool("IsDead", true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            animator_.SetTrigger("Attack");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (transform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,
                              ground_check_distance_);
    }

    public Animator PAnimator() { return animator_; }

    private bool IsGrounded()
    {
        Vector2 check_position = (Vector2)transform.position - new Vector2(0, ground_check_distance_);
        Collider2D hit = Physics2D.OverlapCircle(check_position, ground_check_distance_, ground_layer_);

        Debug.DrawRay(check_position, Vector2.down * ground_check_distance_, Color.red);

        return hit != null;
    }
}
