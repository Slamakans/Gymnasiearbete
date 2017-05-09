using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game {
    public static bool LOADED = false;
    public static bool MAIN_MENU_INITIALIZED = false;
    public static bool HadToConfineCamera;

    public static GameObject Player ()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public static int NUM_LEVELS;

    static Game()
    {
        NUM_LEVELS = SceneManager.sceneCountInBuildSettings;

        Game.LOADED = true;
    }

    /*public static void InitBestTimes()
    {
        for (int i = 0; i < NUM_LEVELS; i++)
        {
            BEST_TIMES[i] = PlayerPrefs.GetFloat("level " + i + " best time", 0f);
        }
    }*/

    public static void SetBestTime(int level, float time)
    {
        PlayerPrefs.SetFloat("level " + level + " best time", time);
    }

    public static string GetBestTimeString(int level)
    {
        // Debug.Log(level);
        string time = PlayerPrefs.GetFloat("level " + level + " best time", 0f).ToString("F2") + "s";
        return time == "0.00s" ? "N/A" : time;
    }


    public static float StartTime = 0;
}
