using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MovingObject {
    private Animator animator;
    private Vector3 spawnPoint;

    public Transform StonePlayer;

    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();
    }

    protected override void Move(Vector2 dir)
    {
        base.Move(dir);
        animator.SetBool("moving", Mathf.Abs(rb2d.velocity.x) > 0.15f);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Stoneify"))
        {
            var stonePlayer = Instantiate(StonePlayer, transform.position, Quaternion.identity, transform.parent);
            transform.position = spawnPoint;
        }
    }
}
