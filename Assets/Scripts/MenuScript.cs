using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    public Button NEW_GAME;
    public Button LEVEL_SELECT;
    public Button HELP;
    public Button CREDITS;
    public Button EXIT_GAME;

    public GameObject PRESS_ANY_KEY_TEXT;

    private GameObject[] buttons;

    private bool initialized = false;

    void Start ()
    {
        buttons = GameObject.FindGameObjectsWithTag("MenuButton");
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    void Update ()
    {
        if (!Game.MAIN_MENU_INITIALIZED && Input.anyKeyDown)
        {
            InitializeMenu();
        }
        else if (Game.MAIN_MENU_INITIALIZED)
        {
            if (!initialized) InitializeMenu();

            if (buttons[0].GetComponent<Image>().fillAmount != 1)
            {
                foreach (GameObject button in buttons)
                {
                    float amt = button.GetComponent<Image>().fillAmount;
                    button.GetComponent<Image>().fillAmount = Mathf.Lerp(amt, 1f, 0.055f);
                }
            }
        }
    }

    void InitializeMenu ()
    {
        initialized = true;
        PRESS_ANY_KEY_TEXT.SetActive(false);
        Game.MAIN_MENU_INITIALIZED = true;

        NEW_GAME.onClick.AddListener(OnNewGame);
        LEVEL_SELECT.onClick.AddListener(OnLevelSelect);
        HELP.onClick.AddListener(OnHelp);
        CREDITS.onClick.AddListener(OnCredits);
        EXIT_GAME.onClick.AddListener(OnExitGame);

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().fillAmount = 0f;
            button.SetActive(true);
        }

        StartCoroutine(EnableEventSystemAfterDelay(0.8f));
    }

    private IEnumerator EnableEventSystemAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().sendNavigationEvents = true;
        yield return new WaitForFixedUpdate();
        NEW_GAME.Select();
    }

    public void OnNewGame ()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnLevelSelect ()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnHelp ()
    {
        SceneManager.LoadScene("Help");
    }

    public void OnCredits ()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnExitGame ()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
