using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public static bool MAIN_MENU_INITIALIZED = false;
    public static GameObject player = GameObject.FindGameObjectWithTag("Player");
    public static bool HadToConfineCamera;
}
