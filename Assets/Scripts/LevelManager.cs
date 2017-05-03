using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {
    public static int LevelReached
    {
        get {
            return PlayerPrefs.GetInt("level reached", 1);
        }

        set
        {
            PlayerPrefs.SetInt("level reached", value);
        }
    }
}
