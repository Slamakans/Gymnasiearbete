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
        // Debug.Log(buttons);
        int index = 0;
        foreach (Button button in buttons)
        {
            button.interactable = index < LevelManager.LevelReached || index + 1 == buttons.Length;
            if (button.name.Contains("Level"))
            {

                // Debug.Log();
                Text timeText = button.transform.Find("TimeText").gameObject.GetComponent<Text>();
                string bestTime = Game.GetBestTimeString(index);
                Debug.Log(bestTime);
                timeText.text = bestTime;
            }

            index++;
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
}
