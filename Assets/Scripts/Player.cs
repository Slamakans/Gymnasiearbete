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

	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        if(h != 0)
        {
            attachedRigidbody.AddForce(Vector2.right * MovementSpeed * h, ForceMode2D.Force);
            var hsign = Mathf.Sign(h);
            var scalesign = Mathf.Sign(transform.localScale.x);
            if(scalesign != hsign)
            {
                var newScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                transform.localScale = newScale;
            }
        }

        animator.SetBool("moving", Mathf.Abs(attachedRigidbody.velocity.x) > 0.15f);
	}
}
