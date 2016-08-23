using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour {
    [SerializeField]
    protected float MovementSpeed = 4f;
    protected Rigidbody2D attachedRigidbody;
    
    protected virtual void Start()
    {
        attachedRigidbody = GetComponent<Rigidbody2D>();
    }

    void Move(Vector2 dir)
    {

    }
}
