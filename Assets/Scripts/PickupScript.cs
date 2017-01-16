using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupScript : MonoBehaviour {
    public int amount = 1;
    public void Reset ()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            player.stoneifications += amount;
            Destroy(gameObject);
        }
    }
}
