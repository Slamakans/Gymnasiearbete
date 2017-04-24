using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {
	void Update () {
		if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
