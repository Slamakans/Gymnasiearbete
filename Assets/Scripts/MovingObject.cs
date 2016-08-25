using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour {
    [SerializeField]
    public float MoveForce = 365f;
    public float MaxSpeed = 5f;
    public float JumpForce = 3f;
    public bool FacingRight = true;
    protected Rigidbody2D rb2d;
    protected bool grounded = false;
    public Transform groundCheck;

    private bool jump = false;
    
    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if(Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (jump)
        {
            rb2d.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            jump = false;
        }

        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    protected virtual void Move(Vector2 dir)
    {
        float h = dir.x;

        if (h * rb2d.velocity.x < MaxSpeed)
            rb2d.AddForce(Vector2.right * h * MoveForce);

        if (Mathf.Abs(rb2d.velocity.x) > MaxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * MaxSpeed, rb2d.velocity.y);

        if (h > 0) FacingRight = true; else if (h < 0) FacingRight = false;

        var scaleSign = Mathf.Sign(transform.localScale.x);
        if ((FacingRight && scaleSign == -1) || (!FacingRight && scaleSign == 1))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}

/*
using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;


    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;


    // Use this for initialization
    void Awake () 
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip ();
        else if (h < 0 && facingRight)
            Flip ();

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
    */
