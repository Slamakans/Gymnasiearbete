using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject {
    private Animator animator;
    private Vector3 spawnPoint;
    private bool stoned = false;

    public bool grabbing = false;

    public Transform StonePlayer;

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    protected override void Move(Vector2 dir)
    {
        if (grabbing) return;
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
            Instantiate(StonePlayer, transform.position, Quaternion.identity, transform.parent);
            transform.position = spawnPoint;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            grabbing = false;
            animator.SetBool("grabbing", grabbing);
        }
        else if (Input.GetButtonDown("Stoneify"))
        {
            animator.SetTrigger("stoneify");
        }
    }
}
