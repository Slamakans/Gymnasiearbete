using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathTrap : MonoBehaviour {
    public void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            Debug.Log("Trying to kill player");
            other.GetComponent<Player>().Kill();
        }
    }
}
