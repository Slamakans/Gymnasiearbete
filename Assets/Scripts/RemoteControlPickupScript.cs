using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RemoteControlPickupScript : MonoBehaviour
{
    public void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Player.HasRemote = true;
        Destroy(gameObject);
    }
}
