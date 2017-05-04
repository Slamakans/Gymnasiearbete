using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour {
    void Start ()
    {
        StartCoroutine(DoTheThing());
    }

    private IEnumerator DoTheThing ()
    {
        float currentTime = 0f;
        float timeToMove = 1.5f;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        yield return new WaitForSecondsRealtime(1.5f);
        yield return new WaitUntil(() =>
        {
            currentTime += Time.deltaTime;
            rend.color = new Color(1, 1, 1, Mathf.Lerp(rend.color.a, 0, currentTime / timeToMove));
            return rend.color.a == 0;
        });

        SceneManager.LoadScene("MainMenu");
    }
}
