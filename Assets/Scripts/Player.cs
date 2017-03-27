using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject
{
    private Animator animator;
    private Vector3 spawnPoint;
    private bool stoned = false;
    [SerializeField]
    private bool sprintJumping = false;

    private float originalGravityScale;
    public float wallSlideGravityScale = 3f;

    public Transform wallCheck;

    public bool grabbing = false;
    public bool touchingWall = false;
	public bool sprinting = false;

    private bool[] canJumpWall = new bool[] { true, true };

    // Use this to connect the new TV to the grabbed TV? idk we gonna have a thunker about this one
    private GameObject grabbedLedge;

    public bool Slides = false;

    public Transform StonePlayer;
    [SerializeField]
    public int stoneifications = 4;

    // Left in for backwards compatibility lmao
    public int GetStoneifications() { return stoneifications; }

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
        originalGravityScale = rb2d.gravityScale;

        Kill();
    }

    internal void SetSpawn(Vector3 position)
    {
        spawnPoint = position;
    }

    protected override void Move(Vector2 dir, float modifier = 1, float force = 0)
    {
        if (grabbing) return;
        if (dir.x == 0 && grounded && !Slides) rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		base.Move(dir, Mathf.Abs(transform.localScale.x / 2) * ((sprinting || sprintJumping) ? 1.75f : 1f), MoveForce); // / (grounded ? 1 : 30));
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.075f);
    }

    protected override void Jump(float modifier = 1)
    {
        int wall = transform.localScale.x < 0 ? 0 : 1;
        bool performWallJump = touchingWall && canJumpWall[wall] & !grabbing && !grounded;

        LetGo();

        base.Jump(Mathf.Abs(transform.localScale.x) / 2 * (performWallJump ? 1f : 1f));

        if (sprinting)
        {
            StartCoroutine(SprintJump());
        }

        if (performWallJump)
        {
            base.Move(new Vector2(-transform.localScale.x, 0), 2);
            touchingWall = false;
            canJumpWall[wall] = false;
            StartCoroutine(ResetWall(wall));
        }
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("grabbing", grabbing);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", rb2d.velocity.y < 0f && !grounded);

        if (transform.position.y < -200)
        {
            Kill();
            return;
        }

        // Debug.Log(sprinting);

        // Debug.Log("left: " + canJumpWall[0] + ",  right: " + canJumpWall[1]);

        touchingWall = !!Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")).collider && canJumpWall[transform.localScale.x < 0 ? 0 : 1];
        animator.SetBool("touching_wall", touchingWall);

		if ((grounded || grabbing || touchingWall) && Input.GetButtonDown("Jump") && rb2d.constraints != RigidbodyConstraints2D.FreezePositionX)
        {
            jump = true;
        }

		sprinting = Mathf.Abs(rb2d.velocity.x) > 0.075f && Input.GetButton("Sprint");
        animator.SetBool("running", sprinting); // consistent af shut up

        rb2d.gravityScale = touchingWall && rb2d.velocity.y <= 0 ? wallSlideGravityScale : originalGravityScale;

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (grabbing && Input.GetButton("Drop Down"))
        {
            LetGo();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stoneify"))
        {
            rb2d.velocity = Vector3.zero;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            stoned = true;
        }
        else if (stoned)
        {
            stoned = false;
            stoneifications -= 1;
            Transform stone = Instantiate(StonePlayer, transform.position, Quaternion.identity, transform.parent) as Transform;
            stone.localScale = transform.localScale;
            transform.position = spawnPoint;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            grabbing = false;
            animator.SetBool("grabbing", grabbing);
        }
        else if (Input.GetButtonDown("Stoneify") && stoneifications > 0)
        {
            animator.SetTrigger("stoneify");
        }
    }

    protected IEnumerator Die()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        animator.ResetTrigger("spawn");
        animator.SetTrigger("die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = spawnPoint;
        yield return new WaitUntil(() => {
            Vector2 a = transform.position;
            Vector2 b = Camera.main.transform.position;
            float distance = (a - b).magnitude;
            return distance < 0.01f;
        });
        yield return new WaitForSeconds(0.15f);
        animator.SetTrigger("spawn");
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitWhile(() => {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") || animator.GetCurrentAnimatorStateInfo(0).IsName("Invisible");
         });
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Kill()
    {
        StartCoroutine(Die());
    }

    // wall == 0 = left, wall == 1 = right
    protected IEnumerator ResetWall(int wall)
    {
        yield return new WaitUntil(() => grabbing || touchingWall || grounded);
        canJumpWall[wall] = true;
    }

    protected void LetGo()
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        grabbing = false;
        animator.SetBool("grabbing", grabbing);
    }

    protected IEnumerator SprintJump()
    {
        bool prev = transform.localScale.x > 0;
        // prev != transform.localScale.x > 0 is true when the orientation changes
        sprintJumping = true;
        yield return new WaitForSeconds(0.15f);
        yield return new WaitUntil(() => grounded || touchingWall || (prev != transform.localScale.x > 0) || grabbing);
        sprintJumping = false;
    }
}
