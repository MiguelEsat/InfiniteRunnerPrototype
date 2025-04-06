using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator_;
    private BoxCollider2D box_collider_;
    private Rigidbody2D rigidbody_;

    [Header("Jump Physics")]
    [SerializeField] private float jump_force_ = 12.0f;
    [SerializeField] private float dash_jump_force_ = 8.0f;
    [SerializeField] private float horizontal_drift_ = 1.0f;
    [SerializeField] private float dash_horizontal_drift_ = 4.0f;

    [Header("Gravity Control")]
    [SerializeField] private float base_gravity_ = 2.0f;
    [SerializeField] private float dash_gravity_multiplier_ = 0.5f;

    [Header("Other Variables")]
    private float dash_timer_;
    public float dash_time;

    [SerializeField] private float jump_hold_time = 0.1f;
    private float jump_hold_timer = 0.0f;

    [SerializeField] private float coyote_time = 0.1f;
    private float coyote_timer = 0.0f;

    private bool is_jumping = false;
    private bool is_grounded = false;
    private bool is_dashing = false;

    [SerializeField] private LayerMask ground_layer_;

    void Start()
    {
        animator_ = GetComponent<Animator>();   
        box_collider_ = GetComponent<BoxCollider2D>();
        rigidbody_ = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
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
        is_dashing = true;
        animator_.SetFloat("Velocity", 2.0f);
    }

    private void DashingTimer()
    {
        if (is_dashing)
        {
            dash_timer_ += Time.deltaTime;
            if (dash_timer_ >= dash_time)
            {
                is_dashing = false;
                dash_timer_ -= dash_timer_;
            }
        }
    }

    private void Jump()
    {
        animator_.SetFloat("VerticalVel", rigidbody_.linearVelocity.y);
        if (is_grounded)
        {
            is_jumping = false;
            animator_.SetBool("IsJumping", false);
            if (Input.GetButtonDown("Jump"))
            {
                jump_hold_timer = jump_hold_time;
            } else
            {
                jump_hold_timer -= Time.deltaTime;
            }

            if (jump_hold_timer > 0.0f)
            {
                animator_.SetFloat("VerticalVel", rigidbody_.linearVelocity.y);

                float current_hori_drift = is_dashing ? dash_horizontal_drift_ : horizontal_drift_;
                float current_jump_force = is_dashing ? dash_jump_force_ : jump_force_;
                rigidbody_.linearVelocity = new Vector2(current_hori_drift, current_jump_force);
                is_jumping = true;

                animator_.SetBool("IsJumping", true);

                jump_hold_timer = 0.0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            is_grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            is_grounded = false;
        }
    }

    private bool IsGrounded()
    {

        return Physics.Raycast(transform.position, Vector3.down, 
                               0.1f, ground_layer_);
    }
}
