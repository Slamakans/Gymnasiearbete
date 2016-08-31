using UnityEngine;
using System.Collections;

public class GrabberScript : MonoBehaviour {
    private Transform player;
    private bool left = false;
    private Collider2D waiting;

    void OnTriggerEnter2D(Collider2D other)
    {
        player = transform.parent;
        var playerScript = player.GetComponent<Player>();
        if (other.tag == "Ledge")
        {
            if (playerScript.IsGrounded()) return;
            if (playerScript.rb2d.velocity.y < 0)
            {
                Grab(other);
            }
            else
            {
                waiting = other;
                StartCoroutine(GrabWhenFalling(other));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == waiting)
            left = true;
    }

    private IEnumerator GrabWhenFalling(Collider2D other)
    {
        yield return new WaitUntil(() => player.GetComponent<Player>().rb2d.velocity.y < 0);
        if (!left)
        {
            Grab(other);
            waiting = null;
        }
    }

    private void Grab(Collider2D other)
    {
        var world = player.parent;

        transform.parent = world;
        player.parent = transform;

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
        player.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

        player.parent = world;
        transform.parent = player;

        player.GetComponent<Player>().grabbing = true;
    }
}