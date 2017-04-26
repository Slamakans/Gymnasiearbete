using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {
	void Update () {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
		if (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Sprint") != 0)
        {
            if (SceneManager.GetActiveScene().name == "Credits" || SceneManager.GetActiveScene().name == "Help")
                SceneManager.LoadScene("MainMenu");
            else if (r.color == Color.white || Input.GetButtonDown("Start"))
                r.color = r.color == Color.white ? new Color(0f, 0f, 0f, 0f) : Color.white;
        }
        else if (r.color == Color.white)
        {
            Vector3 p = Camera.main.transform.position;
            transform.position = new Vector3(p.x, p.y, transform.position.z);
        }
	}
}
