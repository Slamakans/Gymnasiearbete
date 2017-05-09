using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class Goal : MonoBehaviour {
    public int NextLevel;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            LevelManager.LevelReached = Mathf.Max(NextLevel, LevelManager.LevelReached);

            float timeTaken = Time.realtimeSinceStartup - Game.StartTime;
            int level = NextLevel - 1;
            float prevTime = PlayerPrefs.GetFloat("level " + level + " best time", 0f);
            // Debug.Log("Level boi " + level + ", time taken: " + timeTaken);
            // Debug.Log(prevTime);
            if (prevTime == 0 || timeTaken < prevTime)
            {
                PlayerPrefs.SetFloat("level " + level + " best time", timeTaken);
            }

            if (NextLevel == 4)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("Level " + NextLevel);
            }
        }
    }
}
