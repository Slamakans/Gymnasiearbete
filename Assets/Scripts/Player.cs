using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//[RequireComponent(typeof(BoxCollider2D))]
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
    public bool dying = false;

    public float WallJumpForce = 300f;

    private bool[] canJumpWall = new bool[] { true, true };

    // Use this to connect the new TV to the grabbed TV? idk we gonna have a thunker about this one
    private GameObject grabbedLedge;

    public bool Slides = false;

    public Transform StonePlayer;
    // [SerializeField]
    public int stoneifications = 4;

    public static bool HasRemote = false;
    public bool startWithRemote = true;

    public AudioClip spawnSFX;
    public AudioClip warpSFX;
    public AudioClip[] footsteps;
    private AudioSource audioSource;

    // Left in for backwards compatibility lmao
    public int GetStoneifications() { return stoneifications; }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
        originalGravityScale = rb2d.gravityScale;

        if (!HasRemote && startWithRemote) HasRemote = true;

        // Kill();
        // AudioSource.PlayClipAtPoint(spawnSFX, transform.position);
    }

    internal void SetSpawn(Vector3 position)
    {
        spawnPoint = new Vector3(position.x, position.y, transform.position.z);
    }

    protected override void Move(Vector2 dir, float modifier = 1)
    {
        if (grabbing) return;
        if (dir.x == 0 && grounded && !Slides) rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		base.Move(dir, Mathf.Abs(transform.localScale.x / 2) * ((sprinting || sprintJumping) ? 1.35f : 1f)); // / (grounded ? 1 : 30));
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.075f);
    }

    protected override void Jump(float modifier = 1)
    {
        int wall = transform.localScale.x < 0 ? 0 : 1;
        bool performWallJump = touchingWall && canJumpWall[wall] & !grabbing && !grounded;

        LetGo();

        base.Jump(Mathf.Abs(transform.localScale.x) / 2 * (performWallJump ? 1.2f : 1f));

        if (sprinting)
        {
            StartCoroutine(SprintJump());
        }

        if (performWallJump)
        {

            //base.Move(new Vector2(-transform.localScale.x, 0), 2);//, WallJumpForce);
            rb2d.AddForce(new Vector2(-transform.localScale.x * WallJumpForce * 0.6f * Mathf.Abs(Input.GetAxis("Horizontal")), 0), ForceMode2D.Impulse);

            FacingRight = !FacingRight;
            int scaleSign = (int) Mathf.Sign(transform.localScale.x);
            if ((FacingRight && scaleSign == -1) || (!FacingRight && scaleSign == 1))
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            touchingWall = false;
            canJumpWall[wall] = false;
            StartCoroutine(ResetWall(wall));
        }
    }

    private bool tugging;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!touchingWall)
        {
            tugging = false;
            Move(new Vector2(Input.GetAxis("Horizontal"), 0));
        }
        else
        {
            tugging = true;
            StartCoroutine(TuggAway(Input.GetAxis("Horizontal")));
        }
    }

    private IEnumerator TuggAway(float force)
    {
        yield return new WaitForSeconds(0.2f);
        if (tugging)
        {
            Move(Vector2.right * force);
        }
    }

    // private bool spawning = false;
    protected override void Update()
    {

        if (Input.GetKeyDown("escape") || Input.GetButtonDown("Back"))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        base.Update();

        /* Plays sounds when animation fills criterium */
        SpawnSFX();
        WarpSFX();

        /* Plays footstep sounds on certain frames */
        Footstep();

        /* Touching wall logic */
        touchingWall = !!Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")).collider && canJumpWall[transform.localScale.x < 0 ? 0 : 1];
        rb2d.gravityScale = touchingWall && rb2d.velocity.y <= 0 ? wallSlideGravityScale : originalGravityScale;

        /* Sprinting logic */
        sprinting = Mathf.Abs(rb2d.velocity.x) > 0.075f && (Input.GetButton("Sprint") || Input.GetAxis("Sprint") != 0);

        /* Set animation variables */
        animator.SetBool("touching_wall", touchingWall);
        animator.SetBool("grabbing", grabbing);
        animator.SetBool("grounded", grounded);
        animator.SetBool("falling", rb2d.velocity.y < 0f && !grounded);
        animator.SetBool("running", sprinting);

        /* Kill zone */
        if (transform.position.y < -60 && !dying)
        {
            // Debug.Log("dying");
            Kill();
            return;
        }

        /* Jumping logic */
        if ((grounded || grabbing || touchingWall) && Input.GetButtonDown("Jump") && rb2d.constraints != RigidbodyConstraints2D.FreezePositionX)
        {
            jump = true;
        }

        if (grabbing && Input.GetButton("Drop Down"))
        {
            LetGo();
        }

        HandleGettingStoned();

        if (Input.GetKeyDown("t"))
        {
            Kill();
        }
    }

    private void HandleGettingStoned()
    {
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
        else if (!grabbing && Player.HasRemote && Input.GetButtonDown("Stoneify") && stoneifications > 0)
        {
            animator.SetTrigger("stoneify");
        }
    }

    protected IEnumerator Die()
    {
        dying = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        animator.SetBool("grabbing", false);
        animator.ResetTrigger("spawn");
        animator.SetTrigger("die");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        // Die animation has finished

        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = spawnPoint;
        yield return new WaitUntil(() => {
            Vector2 a = transform.position;
            Vector2 b = Camera.main.transform.position;
            float distance = (a - b).magnitude;
            return distance < 0.01f;// || Camera.main.GetComponent<CameraScript>().ConstrainTest();
        });

        // Player is at spawn, and so is camera

        yield return new WaitForSeconds(0.15f);
        animator.SetTrigger("spawn");
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitWhile(() => {
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PlayerInvisible" || animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "PlayerSpawn";
        });
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        dying = false;
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

    private int GetFrame(int numFrames, float time)
    {
        float limited = time % 1;
        return Mathf.FloorToInt(numFrames * limited);
    }

    private void Footstep()
    {
        AnimatorStateInfo curState = animator.GetCurrentAnimatorStateInfo(0);
        int frame = GetFrame(8, curState.normalizedTime);
        if ((
            (curState.IsName("Walking") && (frame == 1 || frame == 5))
            || (curState.IsName("Running") && frame == 1)
            ) && !audioSource.isPlaying && grounded)
        {
            AudioClip footstep = footsteps[UnityEngine.Random.Range(0, footsteps.Length)];
            audioSource.PlayOneShot(footstep, sprinting ? 0.85f : 0.35f);
            // Debug.Log(footstep.name);
        }
    }

    private void SpawnSFX()
    {
        AnimatorStateInfo curState = animator.GetCurrentAnimatorStateInfo(0);
        if (!curState.IsName("Spawn")) return;

        int frame = GetFrame(8, curState.normalizedTime);
        if (frame == 0)
        {
            AudioSource.PlayClipAtPoint(spawnSFX, transform.position);
        }
        // Debug.Log("dying: " + dying);
    }

    private void WarpSFX()
    {
        AnimatorStateInfo curState = animator.GetCurrentAnimatorStateInfo(0);
        if (!curState.IsName("Stoneify")) return;

        int frame = GetFrame(5, curState.normalizedTime);
        if (frame == 0)
        {
            AudioSource.PlayClipAtPoint(warpSFX, transform.position);
        }
    }
}
