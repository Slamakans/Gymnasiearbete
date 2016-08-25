using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject {
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Move(Vector2 dir)
    {
        base.Move(dir);
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.15f);
    }
}
