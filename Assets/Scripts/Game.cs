using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public static bool MAIN_MENU_INITIALIZED = false;
    public static GameObject player = GameObject.FindGameObjectWithTag("Player");
    public static bool HadToConfineCamera;

    public static float[] BEST_TIMES = new float[] { 0f, 0f, 0f, 0f };
    public static string GetBestTimeString(int level)
    {
        string time = BEST_TIMES[level].ToString("F2") + "s";
        return time == "0.00s" ? "N/A" : time;
    }

    public static float StartTime = 0;
}
