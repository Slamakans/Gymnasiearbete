using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour {

    private SpriteRenderer rend;

    void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = new Color(0, 0, 0, 0);
        StartCoroutine(DoTheThing());
    }

    private IEnumerator DoTheThing ()
    {
        float currentTime = 0f;
        float timeToMove = 50f;
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(() =>
        {
            currentTime += Time.deltaTime;
            rend.color = new Color(1, 1, 1, Mathf.Lerp(rend.color.a, 1, currentTime / timeToMove));
            return rend.color.a > 0.99;
        });

        SceneManager.LoadScene("MainMenu");
    }
}
