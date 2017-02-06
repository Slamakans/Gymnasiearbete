using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour
{

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && GetComponent<SpriteRenderer>().color.g != 1f)
        {
            other.GetComponent<Player>().SetSpawn(transform.position);
            GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f);
        }
    }
}
