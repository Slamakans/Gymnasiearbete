using UnityEngine;
using System.Collections;

public class GrabberScript : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ledge")
        {
            var player = transform.parent;
            var world = player.parent;

            transform.parent = world;
            player.parent = transform;

            transform.position = other.transform.position;
            player.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

            player.parent = world;
            transform.parent = player;

            player.GetComponent<Player>().grabbing = true;
        }
    }
}