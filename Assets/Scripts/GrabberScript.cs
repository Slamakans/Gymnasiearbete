using UnityEngine;
using System.Collections;

public class GrabberScript : MonoBehaviour {
    private Transform player;
    private bool left = false;
    private Collider2D waiting;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Enter");
        player = transform.parent;
        var playerScript = player.GetComponent<Player>();
        if (other.tag == "Ledge")
        {
            if (playerScript.IsGrounded()) return;
            // Debug.Log("Not on Ground");
            if (playerScript.rb2d.velocity.y < 0)
            {
                Grab(other);
            }
            else
            {
                // Debug.Log("GOING UP UP UP UP HEYO");
                waiting = other;
                StartCoroutine(GrabWhenFalling(other));
                // Debug.Log("Started Coroutine");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == waiting)
        {
            left = true;
            // Debug.Log("Exit");
        }
    }

    private IEnumerator GrabWhenFalling(Collider2D other)
    {
        // Debug.Log("GrabWhenFalling Start");
        yield return new WaitUntil(() => player.GetComponent<Player>().rb2d.velocity.y < 0);
        if (!left)
        {
            // Debug.Log("FINNA GRAB");
            Grab(other);
            waiting = null;
        }
        // Debug.Log("GrabWhenFalling End");
    }

    private void Grab(Collider2D other)
    {
        if (Input.GetButton("Drop Down")) { return; }
        Transform world = player.parent;

        transform.parent = world;
        player.parent = transform;

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
        player.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

        player.parent = world;
        transform.parent = player;

        player.GetComponent<Player>().grabbing = true;
        // Debug.Log("GRABBED");
    }
}