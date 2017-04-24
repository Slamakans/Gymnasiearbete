using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (!initialized && Input.anyKeyDown)
        {
            PRESS_ANY_KEY_TEXT.SetActive(false);
            initialized = true;
            InitializeMenu();
        }
    }

    void InitializeMenu ()
    {
        NEW_GAME.onClick.AddListener(OnNewGame);
        LEVEL_SELECT.onClick.AddListener(OnLevelSelect);
        HELP.onClick.AddListener(OnHelp);
        CREDITS.onClick.AddListener(OnCredits);
        EXIT_GAME.onClick.AddListener(OnExitGame);

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
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
