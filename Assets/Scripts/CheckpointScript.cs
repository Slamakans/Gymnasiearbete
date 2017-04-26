using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class CheckpointScript : MonoBehaviour
{
    public AudioClip turnOnSFX;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !GetComponent<Animator>().GetBool("idle"))
        {
            other.GetComponent<Player>().SetSpawn(transform.position);
            animator.SetTrigger("turn on");
            animator.SetBool("idle", true);
            AudioSource.PlayClipAtPoint(turnOnSFX, transform.position);
            StartCoroutine(PlayIdleSFX());
        }
    }

    private IEnumerator PlayIdleSFX()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        audioSource.enabled = true;

    }
}
