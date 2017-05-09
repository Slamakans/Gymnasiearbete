using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour {
    public static GameObject MENU_CANVAS;
    public static string CURRENT_SCENE;

    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MenuScript.CANVAS.SetActive(false);
        }
        Cursor.visible = false;
    }

	void Update () {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
		if (Input.anyKeyDown || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Sprint") != 0)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                SceneManager.UnloadSceneAsync(CURRENT_SCENE);
                Cursor.visible = true;
                MenuScript.CANVAS.SetActive(true);
            }
            else if (r.color == Color.white || Input.GetButtonDown("Start"))
            {
                r.color = r.color == Color.white ? new Color(0f, 0f, 0f, 0f) : Color.white;
            }
        }
        else if (r.color == Color.white)
        {
            Vector3 p = Camera.main.transform.position;
            transform.position = new Vector3(p.x, p.y, transform.position.z);
        }
	}
}
