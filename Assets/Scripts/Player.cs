using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject
{
    private Animator animator;
    private Vector3 spawnPoint;
    private bool stoned = false;

    public bool grabbing = false;
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
    }

    protected override void Move(Vector2 dir, float modifier = 1)
    {
        if (grabbing) return;
        if (dir.x == 0 && grounded && !Slides) rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        base.Move(dir, Mathf.Abs(transform.localScale.x / 2));
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.15f);
    }

    protected override void Jump(float modifier = 1)
    {
        base.Jump(Mathf.Abs(transform.localScale.x) / 2);
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("grabbing", grabbing);

        if (Input.GetButtonDown("Jump") && (grounded || grabbing))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (grabbing && (jump || Input.GetButton("Drop Down")))
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            grabbing = false;
            animator.SetBool("grabbing", grabbing);
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
}
