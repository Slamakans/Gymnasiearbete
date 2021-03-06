﻿using UnityEngine;

public class BouncingScript : MonoBehaviour
{
    private Vector3 vel = Vector3.zero;
    private Vector3 origin;

    public float bounceDamping = 1f;
    public float bounceDistance = 1f;

    void Start()
    {
        origin = transform.position;
    }

    private bool down = false;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, origin + new Vector3(0, bounceDistance * (down ? 1 : -1), 0), ref vel, bounceDamping);
        if (Mathf.Abs(origin.y - transform.position.y) >= bounceDistance - 0.3f)
        {
            down = !down;
        }
    }
}
