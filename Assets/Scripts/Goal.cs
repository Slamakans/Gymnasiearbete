using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class Goal : MonoBehaviour {
    public Object NextLevel;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int index = 0;
            string[] arr = NextLevel.name.Split(' ');
            if (int.TryParse(arr[1], out index))
            {
                LevelManager.LevelReached = Mathf.Max(index, LevelManager.LevelReached);

                float timeTaken = Time.realtimeSinceStartup - Game.StartTime;
                int level = index - 1;
                float prevTime = PlayerPrefs.GetFloat("level " + level + " best time", 0f);
                Debug.Log("Level boi " + level + ", time taken: " + timeTaken);
                Debug.Log(prevTime);
                if (prevTime == 0 || timeTaken < prevTime)
                {
                    PlayerPrefs.SetFloat("level " + level + " best time", timeTaken);
                }
            }
            SceneManager.LoadScene(NextLevel.name);
        }
    }
}
