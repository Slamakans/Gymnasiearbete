using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject
{
    private Animator animator;
    private Vector3 spawnPoint;
    private bool stoned = false;

    public bool grabbing = false;
    private GameObject grabbedLedge; // Use this to connect the new TV to the grabbed TV? idk we gonna have a thunker about this one

    public bool Slides = false;

    public Transform StonePlayer;
    [SerializeField]
    private int stoneifications = 4;
    public int GetStoneifications() { return stoneifications; }

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    protected override void Move(Vector2 dir)
    {
        if (grabbing) return;
        if (dir.x == 0 && grounded && !Slides) rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        base.Move(dir);
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.15f);
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

        if (grabbing && jump)
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
            Instantiate(StonePlayer, transform.position, Quaternion.identity, transform.parent);
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
