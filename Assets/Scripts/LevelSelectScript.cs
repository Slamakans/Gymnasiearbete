using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelSelectScript : MonoBehaviour {

    private Button[] buttons;

    void Start ()
    {
        MenuScript.CANVAS.SetActive(false);
        Cursor.visible = true;
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
                if (bestTime != "N/A")
                {
                    timeText.color = Color.white;
                }
                else
                {
                    timeText.color = new Color(188, 188, 188, 255) / 255f;
                }

                index++;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GoBack();
        }
    }

    public void Goto (Button button)
    {
        if (button.name == "MainMenu")
        {
            GoBack();
            return;
        }
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

    private void GoBack ()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.UnloadSceneAsync("LevelSelect");
            MenuScript.CANVAS.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
