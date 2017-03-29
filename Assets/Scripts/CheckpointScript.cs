using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class CheckpointScript : MonoBehaviour
{

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !GetComponent<Animator>().GetBool("idle"))
        {
            other.GetComponent<Player>().SetSpawn(transform.position);
            GetComponent<Animator>().SetTrigger("turn on");
            GetComponent<Animator>().SetBool("idle", true);
        }
    }
}
