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

    public static GameObject CANVAS;

    public GameObject PRESS_ANY_KEY_TEXT;

    private GameObject[] buttons;

    private bool initialized = false;

    void Start ()
    {
        CANVAS = GameObject.Find("Canvas");
        Cursor.visible = true;

        buttons = GameObject.FindGameObjectsWithTag("MenuButton");
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }

        // StartCoroutine(Load());
        if (!Game.LOADED)
        {
            GameObject.Find("Canvas").SetActive(false);

            StartCoroutine(Load());
        }
    }

    private IEnumerator Load ()
    {
        yield return new WaitUntil(() => Game.LOADED);
        GameObject.Find("Canvas").SetActive(true);
    }

    /*private IEnumerator Load()
    {
        if (Game.LOADED) yield break;

        Debug.Log("Loading: " + Time.realtimeSinceStartup);
        yield return StartCoroutine(LoadScenes());
        Debug.Log("Loading finished: " + Time.realtimeSinceStartup);
        Game.LOADED = true;
        GameObject.Find("Canvas").SetActive(true);
    }

    private IEnumerator LoadScenes ()
    {
        bool valid = true;
        for (int i = 1; valid; i++)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync("Level " + i);
            op.allowSceneActivation = false;
            yield return new WaitUntil(() => op.progress == 0.9f);

            Debug.Log("Loaded level " + i);
            valid = SceneManager.GetSceneByName("Level " + i).IsValid();
            Debug.Log("valid: " + valid);
        }

        SceneManager.LoadScene("MainMenu");
        Game.InitBestTimes();
    }*/

    void Update ()
    {
        if (!Game.LOADED) return;

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

            if (Input.GetButtonDown("Back"))
            {
                OnExitGame();
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
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Additive);
    }

    public void OnHelp ()
    {
        CreditsScript.CURRENT_SCENE = "Help";

        SceneManager.LoadScene(CreditsScript.CURRENT_SCENE, LoadSceneMode.Additive);
    }

    public void OnCredits ()
    {
        CreditsScript.CURRENT_SCENE = "Credits";

        SceneManager.LoadScene(CreditsScript.CURRENT_SCENE, LoadSceneMode.Additive);
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
