using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour {
    [SerializeField]
    public float MoveForce = 365f;
    public float MaxSpeed = 5f;
    public float JumpForce = 20f;
    public bool FacingRight = true;
    public Rigidbody2D rb2d;
    protected bool grounded = false;
    public Transform groundCheck_1;
    public Transform groundCheck_2;

    protected bool jump = false;

    public bool IsGrounded() { return grounded; }
    
    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        grounded = !!Physics2D.OverlapArea(groundCheck_1.position, groundCheck_2.position, 1 << LayerMask.NameToLayer("Ground"));
    }

    protected virtual void FixedUpdate()
    {
        if (jump)
        {
            Jump();
        }

        Move(new Vector2(Input.GetAxis("Horizontal"), 0));
    }

    protected virtual void Jump(float modifier = 1)
    {
        rb2d.AddForce(new Vector2(0f, JumpForce * modifier), ForceMode2D.Impulse);
        jump = false;
    }

    protected virtual void Move(Vector2 dir, float modifier = 1)
    {
        float h = dir.x;

        if (h != 0)
            rb2d.AddForce(Vector2.right * h * MoveForce * modifier);

        if (Mathf.Abs(rb2d.velocity.x) > MaxSpeed * modifier)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * MaxSpeed * modifier, rb2d.velocity.y);

        if (h > 0) FacingRight = true; else if (h < 0) FacingRight = false;

        var scaleSign = Mathf.Sign(transform.localScale.x);
        if ((FacingRight && scaleSign == -1) || (!FacingRight && scaleSign == 1))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}