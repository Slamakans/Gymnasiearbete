using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour {

    private Button[] buttons;

    void Start ()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    void Update ()
    {
        int index = 0;
        foreach (Button button in buttons)
        {
            button.interactable = index < LevelManager.LevelReached || button.name == "MainMenu" || button.name == "Reset";
            if (button.name.Contains("Level"))
            {
                Text timeText = button.transform.Find("TimeText").gameObject.GetComponent<Text>();
                string bestTime = Game.GetBestTimeString(index + 1);
                timeText.text = bestTime;

                index++;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Goto (Button button)
    {
        SceneManager.LoadScene(button.name);
    }

    public void ResetProgress ()
    {
        Debug.Log("num levels: " + Game.NUM_LEVELS);
        for (int i = 0; i < Game.NUM_LEVELS; i++)
        {
            Game.SetBestTime(i + 1, 0f);
            LevelManager.LevelReached = 1;

            // SceneManager.LoadScene("LevelSelect");
        }
    }
}
