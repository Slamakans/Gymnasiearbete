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
            if (int.TryParse(arr[Random.Range(0, arr.Length)], out index))
            {
                LevelManager.LevelReached = Mathf.Max(index, LevelManager.LevelReached);
            }
            SceneManager.LoadScene(NextLevel.name);
        }
    }
}
