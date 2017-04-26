using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour {
    public float MaxSpeed = 20f;
    public float JumpForce = 20f;
    public bool FacingRight = true;
    public Rigidbody2D rb2d;

    [SerializeField]
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
        Vector2 prev = rb2d.velocity;
        rb2d.velocity = new Vector2(prev.x, JumpForce * modifier);
        jump = false;
    }

    protected virtual void Move(Vector2 dir, float modifier = 1)
    {
        float h = dir.x;

        if (h == 0) return;

        bool slow = Mathf.Abs(rb2d.velocity.x) <= MaxSpeed * modifier;

        if (!grounded)
        {
            float x = rb2d.velocity.x;
            float min = slow ? -MaxSpeed * modifier : -Mathf.Abs(rb2d.velocity.x) ;
            float max = slow ? MaxSpeed * modifier : Mathf.Abs(rb2d.velocity.x);
            Debug.Log("Min: " + min + ", Max: " + max);
            rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x + h * modifier * 3f, min, max), rb2d.velocity.y);
            Debug.Log("velocity 1: " + rb2d.velocity);
        }
        else if (slow)
        {
            //rb2d.AddForce(Vector2.right * h * force * modifier);
            rb2d.velocity = new Vector2(h * MaxSpeed * modifier, rb2d.velocity.y);
            Debug.Log("velocity 2: " + rb2d.velocity);
        }

        FacingRight = rb2d.velocity.x > 0;

        var scaleSign = Mathf.Sign(transform.localScale.x);
        if ((FacingRight && scaleSign == -1) || (!FacingRight && scaleSign == 1))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}